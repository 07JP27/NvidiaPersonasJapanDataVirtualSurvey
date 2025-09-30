using System.Text.Json.Serialization;

namespace NvidiaPersonasJapanDataVirtualSurvey.Core.Models;

internal record BatchRequestItem
{
    [JsonPropertyName("custom_id")]
    public string CustomId { get; init; }

    [JsonPropertyName("method")]
    public string Method { get; init; } = "POST";

    [JsonPropertyName("url")]
    public string Url { get; init; } = "/chat/completions";

    [JsonPropertyName("body")]
    public BatchRequestBody Body { get; init; }

    public BatchRequestItem(BatchRequestBody Body, string? CustomId = null, string? Method = null, string? Url = null)
    {
        this.CustomId = CustomId ?? Guid.NewGuid().ToString();
        this.Method = Method ?? "POST";
        this.Url = Url ?? "/chat/completions";
        this.Body = Body;
    }
}
