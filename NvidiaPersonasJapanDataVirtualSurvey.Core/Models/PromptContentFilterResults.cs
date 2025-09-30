using System.Text.Json.Serialization;

namespace NvidiaPersonasJapanDataVirtualSurvey.Core.Models;

internal record PromptContentFilterResults
{
    [JsonPropertyName("hate")]
    public FilterResult? Hate { get; init; }

    [JsonPropertyName("self_harm")]
    public FilterResult? SelfHarm { get; init; }

    [JsonPropertyName("sexual")]
    public FilterResult? Sexual { get; init; }

    [JsonPropertyName("violence")]
    public FilterResult? Violence { get; init; }

    [JsonPropertyName("jailbreak")]
    public JailbreakFilterResult? Jailbreak { get; init; }
}
