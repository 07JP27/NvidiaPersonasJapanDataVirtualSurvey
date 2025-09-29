﻿using System.Collections;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using NvidiaPersonasJapanDataVirtualSurvey.Core.Models;
using Parquet;
using Parquet.Data;

namespace NvidiaPersonasJapanDataVirtualSurvey.Core;

internal class DataLoader(IProgress<string>? progress = null)
{
    const string dataset = "nvidia/Nemotron-Personas-Japan"; // 対象データセット
    static readonly string? subset = null;   // 例: "default"。未指定なら全subsetから拾う
    const string split = "train"; // 例: train / test / validation

    const string dataDirName = "PersonasData";


    internal async Task<IReadOnlyList<PersonaRecord>> LoadAsync(int sampleSize = 0)
    {
        // TODO: 引数でサンプリング時のフィルター条件を指定できるようにする

        var files = CollectParquetFiles(dataDirName);
        if (files.Count == 0)
        {
            progress?.Report("Local Parquet files were not found.");
            await DownloadDataAsync();
            files = CollectParquetFiles(dataDirName);
            if (files.Count == 0)
            {
                progress?.Report($"No Parquet files available after download attempt under: {dataDirName}");
                return Array.Empty<PersonaRecord>();
            }
        }
        else
        {
            progress?.Report($"Found {files.Count} local Parquet files.");
        }

        var records = new List<PersonaRecord>();

        foreach (var file in files)
        {
            using var stream = File.OpenRead(file);
            using var parquetReader = await ParquetReader.CreateAsync(stream);
            var dataFields = parquetReader.Schema.DataFields;
            if (dataFields.Length == 0)
            {
                continue;
            }

            for (int i = 0; i < parquetReader.RowGroupCount; i++)
            {
                using var groupReader = parquetReader.OpenRowGroupReader(i);
                var columns = new Dictionary<string, Array>(StringComparer.OrdinalIgnoreCase);

                foreach (var field in dataFields)
                {
                    var column = await groupReader.ReadColumnAsync(field);
                    columns[field.Name] = column.Data;
                }

                var rowCount = columns[dataFields[0].Name].Length;

                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    var record = new PersonaRecord(
                        Uuid: columns.GetString("uuid", rowIndex) ?? string.Empty,
                        ProfessionalPersona: columns.GetString("professional_persona", rowIndex),
                        SportsPersona: columns.GetString("sports_persona", rowIndex),
                        ArtsPersona: columns.GetString("arts_persona", rowIndex),
                        TravelPersona: columns.GetString("travel_persona", rowIndex),
                        CulinaryPersona: columns.GetString("culinary_persona", rowIndex),
                        Persona: columns.GetString("persona", rowIndex),
                        CulturalBackground: columns.GetString("cultural_background", rowIndex),
                        SkillsAndExpertise: columns.GetString("skills_and_expertise", rowIndex),
                        SkillsAndExpertiseList: columns.GetStringList("skills_and_expertise_list", rowIndex),
                        HobbiesAndInterests: columns.GetString("hobbies_and_interests", rowIndex),
                        HobbiesAndInterestsList: columns.GetStringList("hobbies_and_interests_list", rowIndex),
                        CareerGoalsAndAmbitions: columns.GetString("career_goals_and_ambitions", rowIndex),
                        Sex: columns.GetString("sex", rowIndex),
                        Age: columns.GetNullableInt("age", rowIndex),
                        MaritalStatus: columns.GetString("marital_status", rowIndex),
                        EducationLevel: columns.GetString("education_level", rowIndex),
                        Occupation: columns.GetString("occupation", rowIndex),
                        Region: columns.GetString("region", rowIndex),
                        Area: columns.GetString("area", rowIndex),
                        Prefecture: columns.GetString("prefecture", rowIndex),
                        Country: columns.GetString("country", rowIndex)
                    );

                    records.Add(record);
                }
            }
        }

        if (sampleSize <= 0 || sampleSize >= records.Count)
        {
            progress?.Report($"Collected and returned {records.Count} persona records.");
            return records;
        }
        else
        {
            // TODO:全件メモリにロードしてからサンプリングするのは効率的ではないので、本来は読み込み時にサンプリングしたい（複数ファイルに分かれてるので完全な一様性を保ったままサンプリングするのは難しそう）

            // 部分的な Fisher–Yates シャッフル
            for (int i = 0; i < sampleSize; i++)
            {
                int j = Random.Shared.Next(i, records.Count);
                (records[i], records[j]) = (records[j], records[i]);
            }
            progress?.Report($"Collected {records.Count} persona records (returning {sampleSize}).");
            return records.Take(sampleSize).ToList();
        }
    }

    internal async Task DownloadDataAsync()
    {
        progress?.Report("Downloading dataset from remote...");

        using var http = new HttpClient();

        var api = $"https://datasets-server.huggingface.co/parquet?dataset={Uri.EscapeDataString(dataset)}";
        var json = await http.GetStringAsync(api);

        var payload = JsonSerializer.Deserialize<ParquetListResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        if (payload?.ParquetFiles is null || payload.ParquetFiles.Count == 0)
        {
            Console.Error.WriteLine("Parquet files not found for dataset.");
            return;
        }

        var files = payload.ParquetFiles
        .Where(p => string.Equals(p.Split, split, StringComparison.OrdinalIgnoreCase)
            && (subset is null || string.Equals(p.Config, subset, StringComparison.OrdinalIgnoreCase)))
            .ToList();
        if (files.Count == 0) files = payload.ParquetFiles.ToList();

        Directory.CreateDirectory(dataDirName);
        progress?.Report($"{files.Count} Parquet files to download.");

        foreach (var parquet in files)
        {
            var localPath = Path.Combine(dataDirName, parquet.Filename);

            if (File.Exists(localPath))
            {
                progress?.Report($"Skip (already exists): {localPath}");
                continue;
            }

            await using (var src = await http.GetStreamAsync(parquet.Url))
            await using (var dst = File.Create(localPath))
            {
                await src.CopyToAsync(dst);
            }

            progress?.Report($"Saved: {localPath}");
        }
        progress?.Report("Downloading completed.");
        
    }

    private List<string> CollectParquetFiles(string dataDirName)
    {
        if (!Directory.Exists(dataDirName)) return new List<string>();
        return Directory.EnumerateFiles(dataDirName, "*.parquet", SearchOption.TopDirectoryOnly).ToList();
    }
}