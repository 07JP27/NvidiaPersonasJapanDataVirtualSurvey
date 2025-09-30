using System.Text.Json.Serialization;
using NvidiaPersonasJapanDataVirtualSurvey.Core.Converters;

namespace NvidiaPersonasJapanDataVirtualSurvey.Core.Models;

internal record BatchStatus
{
    [JsonPropertyName("cancelled_at")]
    [JsonConverter(typeof(NullableUnixTimestampConverter))]
    public DateTime? CancelledAt { get; init; }

    [JsonPropertyName("cancelling_at")]
    [JsonConverter(typeof(NullableUnixTimestampConverter))]
    public DateTime? CancellingAt { get; init; }

    [JsonPropertyName("completed_at")]
    [JsonConverter(typeof(NullableUnixTimestampConverter))]
    public DateTime? CompletedAt { get; init; }

    [JsonPropertyName("completion_window")]
    public string CompletionWindow { get; init; } = string.Empty;

    [JsonPropertyName("created_at")]
    [JsonConverter(typeof(UnixTimestampConverter))]
    public DateTime CreatedAt { get; init; }

    [JsonPropertyName("error_file_id")]
    public string? ErrorFileId { get; init; }

    [JsonPropertyName("expired_at")]
    [JsonConverter(typeof(NullableUnixTimestampConverter))]
    public DateTime? ExpiredAt { get; init; }

    [JsonPropertyName("expires_at")]
    [JsonConverter(typeof(UnixTimestampConverter))]
    public DateTime ExpiresAt { get; init; }

    [JsonPropertyName("failed_at")]
    [JsonConverter(typeof(NullableUnixTimestampConverter))]
    public DateTime? FailedAt { get; init; }

    [JsonPropertyName("finalizing_at")]
    [JsonConverter(typeof(NullableUnixTimestampConverter))]
    public DateTime? FinalizingAt { get; init; }

    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    [JsonPropertyName("in_progress_at")]
    [JsonConverter(typeof(NullableUnixTimestampConverter))]
    public DateTime? InProgressAt { get; init; }

    [JsonPropertyName("input_file_id")]
    public string InputFileId { get; init; } = string.Empty;

    [JsonPropertyName("errors")]
    public object? Errors { get; init; }

    [JsonPropertyName("metadata")]
    public object? Metadata { get; init; }

    [JsonPropertyName("object")]
    public string Object { get; init; } = string.Empty;

    [JsonPropertyName("output_file_id")]
    public string? OutputFileId { get; init; }

    [JsonPropertyName("request_counts")]
    public RequestCounts? RequestCounts { get; init; }

    [JsonPropertyName("status")]
    public string Status { get; init; } = string.Empty;
}
