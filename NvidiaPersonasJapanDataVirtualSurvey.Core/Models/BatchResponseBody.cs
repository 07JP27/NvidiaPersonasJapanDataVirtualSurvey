using System.Text.Json.Serialization;
using NvidiaPersonasJapanDataVirtualSurvey.Core.Converters;

namespace NvidiaPersonasJapanDataVirtualSurvey.Core.Models;

internal record BatchResponseBody
{
    [JsonPropertyName("choices")]
    public Choice[]? Choices { get; init; }

    [JsonPropertyName("created")]
    [JsonConverter(typeof(UnixTimestampConverter))]
    public DateTime Created { get; init; }

    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    [JsonPropertyName("model")]
    public string Model { get; init; } = string.Empty;

    [JsonPropertyName("object")]
    public string Object { get; init; } = string.Empty;

    [JsonPropertyName("system_fingerprint")]
    public string? SystemFingerprint { get; init; }

    [JsonPropertyName("usage")]
    public Usage? Usage { get; init; }

    [JsonPropertyName("prompt_filter_results")]
    public PromptFilterResult[]? PromptFilterResults { get; init; }
}
