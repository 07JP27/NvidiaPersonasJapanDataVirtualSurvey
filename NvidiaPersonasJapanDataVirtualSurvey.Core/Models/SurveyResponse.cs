using System.Text.Json.Serialization;

namespace NvidiaPersonasJapanDataVirtualSurvey.Core.Models;

public class SurveyResponse
{
    public List<PersonaAnswer> Answers { get; set; } = new List<PersonaAnswer>();
    public UsageToken Usage { get; set; } = new UsageToken();
}

public record PersonaAnswer(PersonaRecord Persona ,string Answer);

public class UsageToken
{
    public int CompletionTokens { get; set; } = 0;
    public int PromptTokens { get; set; } = 0;
    public int TotalTokens => CompletionTokens + PromptTokens;
}