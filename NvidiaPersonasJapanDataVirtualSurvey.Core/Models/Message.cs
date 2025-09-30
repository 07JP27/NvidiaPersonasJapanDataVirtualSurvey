using System.Text.Json.Serialization;

namespace NvidiaPersonasJapanDataVirtualSurvey.Core.Models;

internal record Message
{
    [JsonPropertyName("role")]
    public string Role { get; init; } = string.Empty;

    [JsonPropertyName("content")]
    public string? Content { get; init; }

    [JsonPropertyName("refusal")]
    public string? Refusal { get; init; }

    [JsonPropertyName("annotations")]
    public object[]? Annotations { get; init; }
}
