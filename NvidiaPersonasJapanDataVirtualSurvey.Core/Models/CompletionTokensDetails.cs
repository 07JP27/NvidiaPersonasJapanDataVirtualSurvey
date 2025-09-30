using System.Text.Json.Serialization;

namespace NvidiaPersonasJapanDataVirtualSurvey.Core.Models;

internal record CompletionTokensDetails
{
    [JsonPropertyName("accepted_prediction_tokens")]
    public int AcceptedPredictionTokens { get; init; }

    [JsonPropertyName("audio_tokens")]
    public int AudioTokens { get; init; }

    [JsonPropertyName("reasoning_tokens")]
    public int ReasoningTokens { get; init; }

    [JsonPropertyName("rejected_prediction_tokens")]
    public int RejectedPredictionTokens { get; init; }
}
