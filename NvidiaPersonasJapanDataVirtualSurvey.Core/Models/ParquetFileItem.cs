using System.Text.Json.Serialization;

namespace NvidiaPersonasJapanDataVirtualSurvey.Core.Models;

/// <summary>
/// Represents a single Parquet file metadata entry returned by the Hugging Face datasets server.
/// </summary>
/// <param name="Dataset">Dataset identifier.</param>
/// <param name="Config">Subset / config name.</param>
/// <param name="Split">Data split (e.g. train / test / validation).</param>
/// <param name="Url">Direct download URL for the Parquet file.</param>
/// <param name="Filename">Local filename suggested by server.</param>
/// <param name="Size">Approximate size in bytes.</param>
public sealed record class ParquetFileItem(
    string Dataset,
    string Config,
    string Split,
    string Url,
    string Filename,
    long Size
);
