using System.Text.Json.Serialization;

namespace NvidiaPersonasJapanDataVirtualSurvey.Core.Models;

internal record FilterResult
{
    [JsonPropertyName("filtered")]
    public bool Filtered { get; init; }

    [JsonPropertyName("severity")]
    public string? Severity { get; init; }
}
