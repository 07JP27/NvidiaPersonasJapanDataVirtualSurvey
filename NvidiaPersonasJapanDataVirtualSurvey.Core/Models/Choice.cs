using System.Text.Json.Serialization;

namespace NvidiaPersonasJapanDataVirtualSurvey.Core.Models;

internal record Choice
{
    [JsonPropertyName("finish_reason")]
    public string? FinishReason { get; init; }

    [JsonPropertyName("index")]
    public int Index { get; init; }

    [JsonPropertyName("message")]
    public Message? Message { get; init; }

    [JsonPropertyName("logprobs")]
    public object? Logprobs { get; init; }

    [JsonPropertyName("content_filter_results")]
    public ContentFilterResults? ContentFilterResults { get; init; }
}
