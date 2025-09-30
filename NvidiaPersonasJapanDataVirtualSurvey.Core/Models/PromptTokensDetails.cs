using System.Text.Json.Serialization;

namespace NvidiaPersonasJapanDataVirtualSurvey.Core.Models;

internal record PromptTokensDetails
{
    [JsonPropertyName("audio_tokens")]
    public int AudioTokens { get; init; }

    [JsonPropertyName("cached_tokens")]
    public int CachedTokens { get; init; }
}
