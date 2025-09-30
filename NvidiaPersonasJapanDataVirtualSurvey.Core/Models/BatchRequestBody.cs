using System.Text.Json.Serialization;

namespace NvidiaPersonasJapanDataVirtualSurvey.Core.Models;

internal record BatchRequestBody
{
    [JsonPropertyName("model")]
    public string Model { get; init; }

    [JsonPropertyName("messages")]
    public ChatMessage[] Messages { get; init; }

    public BatchRequestBody(string Model, ChatMessage[] Messages)
    {
        this.Model = Model;
        this.Messages = Messages;
    }
}
