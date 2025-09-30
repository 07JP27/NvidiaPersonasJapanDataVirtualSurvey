using System.Text.Json.Serialization;

namespace NvidiaPersonasJapanDataVirtualSurvey.Core.Models;

internal record BatchResponse
{
    [JsonPropertyName("body")]
    public BatchResponseBody? Body { get; init; }

    [JsonPropertyName("request_id")]
    public string? RequestId { get; init; }

    [JsonPropertyName("status_code")]
    public int? StatusCode { get; init; }
}
