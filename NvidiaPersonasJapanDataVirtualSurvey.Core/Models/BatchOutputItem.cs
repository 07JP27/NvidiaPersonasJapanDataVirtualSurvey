using System.Text.Json.Serialization;

namespace NvidiaPersonasJapanDataVirtualSurvey.Core.Models;

internal record BatchOutputItem
{
    [JsonPropertyName("custom_id")]
    public string CustomId { get; init; } = string.Empty;

    [JsonPropertyName("response")]
    public BatchResponse? Response { get; init; }

    [JsonPropertyName("error")]
    public object? Error { get; init; }
}
