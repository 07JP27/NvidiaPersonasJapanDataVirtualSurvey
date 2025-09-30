using System.Text.Json.Serialization;

namespace NvidiaPersonasJapanDataVirtualSurvey.Core.Models;

internal record PromptFilterResult
{
    [JsonPropertyName("prompt_index")]
    public int PromptIndex { get; init; }

    [JsonPropertyName("content_filter_results")]
    public PromptContentFilterResults? ContentFilterResults { get; init; }
}
