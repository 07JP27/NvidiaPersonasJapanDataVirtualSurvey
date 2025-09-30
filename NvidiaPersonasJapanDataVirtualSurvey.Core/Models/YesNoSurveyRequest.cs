
namespace NvidiaPersonasJapanDataVirtualSurvey.Core.Models;

public class YesNoSurveyRequest(string query) : ISurveyRequest
{
    public string GetUserPrompt() => query;

    public string GetSystemPrompt(PersonaRecord persona)
    {
        // 2バイト文字だと指定した文字数の半分くらいになるので、倍にしておく
        return $"""
        以下のペルソナになりきって、ユーザーから送信される質問にYes or Noのどちらかで答えてください。
        回答はYesまたはNoのみを返してください。理由などは不要です。

        ペルソナ情報:
        - 職業的なペルソナ: {persona.ProfessionalPersona ?? "情報なし"}
        - スポーツに関するペルソナ属性: {persona.SportsPersona ?? "情報なし"}
        - 芸術に関するペルソナ属性: {persona.ArtsPersona ?? "情報なし"}
        - 旅行に関するペルソナ属性: {persona.TravelPersona ?? "情報なし"}
        - 食文化に関するペルソナ属性: {persona.CulinaryPersona ?? "情報なし"}
        - 全体的なペルソナの要約: {persona.Persona ?? "情報なし"}
        - 文化的背景: {persona.CulturalBackground ?? "情報なし"}
        - スキル・専門性: {persona.SkillsAndExpertise ?? "情報なし"}
        - 趣味・関心事: {persona.HobbiesAndInterests ?? "情報なし"}
        - キャリアの目標および志向: {persona.CareerGoalsAndAmbitions ?? "情報なし"}
        - 性別: {persona.Sex ?? "情報なし"}
        - 年齢: {(persona.Age.HasValue ? persona.Age.Value.ToString() : "情報なし")}
        - 婚姻状況: {persona.MaritalStatus ?? "情報なし"}
        - 最終学歴: {persona.EducationLevel ?? "情報なし"}
        - 現在の職業: {persona.Occupation ?? "情報なし"}
        - 地域: {persona.Region ?? "情報なし"}
        - エリア: {persona.Area ?? "情報なし"}
        - 都道府県: {persona.Prefecture ?? "情報なし"}
        - 国: {persona.Country ?? "情報なし"}
        """;
        
    }
}
