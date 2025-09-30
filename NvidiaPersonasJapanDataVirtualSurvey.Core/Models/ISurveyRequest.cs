
namespace NvidiaPersonasJapanDataVirtualSurvey.Core.Models;

public interface ISurveyRequest
{
    string GetUserPrompt();
    string GetSystemPrompt(PersonaRecord persona);
}
