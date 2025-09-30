using System.Text.Json.Serialization;

namespace NvidiaPersonasJapanDataVirtualSurvey.Core.Models;

internal record JailbreakFilterResult
{
    [JsonPropertyName("filtered")]
    public bool Filtered { get; init; }

    [JsonPropertyName("detected")]
    public bool Detected { get; init; }
}
