using System.Text.Json.Serialization;

namespace NvidiaPersonasJapanDataVirtualSurvey.Core.Models;

internal record RequestCounts
{
    [JsonPropertyName("total")]
    public int? Total { get; init; }

    [JsonPropertyName("completed")]
    public int? Completed { get; init; }

    [JsonPropertyName("failed")]
    public int? Failed { get; init; }
}
