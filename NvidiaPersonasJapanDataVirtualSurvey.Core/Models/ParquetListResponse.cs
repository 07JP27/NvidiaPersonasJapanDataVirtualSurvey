using System.Text.Json.Serialization;

namespace NvidiaPersonasJapanDataVirtualSurvey.Core.Models;

/// <summary>
/// Root response object listing available Parquet files for a dataset.
/// </summary>
public sealed record class ParquetListResponse
{
    [JsonPropertyName("parquet_files")] public List<ParquetFileItem> ParquetFiles { get; init; } = new();
}
