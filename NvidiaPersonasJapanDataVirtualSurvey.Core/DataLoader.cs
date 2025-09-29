using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using Parquet;
using Parquet.Data;

namespace NvidiaPersonasJapanDataVirtualSurvey.Core;

public class DataLoader(IProgress<string>? progress = null)
{
    const string dataset = "nvidia/Nemotron-Personas-Japan"; // 対象データセット
    static readonly string? subset = null;   // 例: "default"。未指定なら全subsetから拾う
    const string split = "train"; // 例: train / test / validation

    const string dataDirName = "PersonasData";


    public async Task LoadAsync()
    {
        var files = CollectParquetFiles(dataDirName);
        if (files.Count == 0)
        {
            progress?.Report("Local Parquet files were not found.");
            await DownloadDataAsync();
            files = CollectParquetFiles(dataDirName);
            if (files.Count == 0)
            {
                progress?.Report($"No Parquet files available after download attempt under: {dataDirName}");
                return;
            }
        }
        else
        {
            progress?.Report($"Found {files.Count} local Parquet files.");
        }

        foreach (var file in files)
        {
            using var stream = File.OpenRead(file);
            using var parquetReader = await ParquetReader.CreateAsync(stream);
            var dataFields = parquetReader.Schema.DataFields;

            for (int i = 0; i < parquetReader.RowGroupCount; i++)
            {
                using var groupReader = parquetReader.OpenRowGroupReader(i);
                foreach (var field in dataFields)
                {
                    var column = await groupReader.ReadColumnAsync(field);
                    progress?.Report($"Read {column.Data.Length} rows from column '{field.Name}' in file '{file}'");
                }
            }
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

public sealed class ParquetListResponse
{
    [JsonPropertyName("parquet_files")]
    public List<ParquetFileItem> ParquetFiles { get; set; } = new();
}

public sealed class ParquetFileItem
{
    public string Dataset { get; set; } = string.Empty;
    public string Config { get; set; } = string.Empty;
    public string Split { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Filename { get; set; } = string.Empty;
    public long Size { get; set; }
}