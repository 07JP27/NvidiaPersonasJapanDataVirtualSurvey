using System.Text.Json.Serialization;

namespace NvidiaPersonasJapanDataVirtualSurvey.Core.Models;

internal record ChatMessage
{
    [JsonPropertyName("role")]
    public string Role { get; init; }

    [JsonPropertyName("content")]
    public string Content { get; init; }

    public ChatMessage(string Role, string Content)
    {
        this.Role = Role;
        this.Content = Content;
    }
}
