namespace NvidiaPersonasJapanDataVirtualSurvey.Core.Models;

/// <summary>
/// Nemotron Personas Japan データセットからエクスポートされた 1 行分のペルソナ情報を表します。
/// </summary>
/// <param name="Uuid">レコードを一意に識別する UUID。</param>
/// <param name="ProfessionalPersona">職業的なペルソナの説明。</param>
/// <param name="SportsPersona">スポーツに関するペルソナ属性。</param>
/// <param name="ArtsPersona">芸術に関するペルソナ属性。</param>
/// <param name="TravelPersona">旅行に関するペルソナ属性。</param>
/// <param name="CulinaryPersona">食文化に関するペルソナ属性。</param>
/// <param name="Persona">全体的なペルソナの要約。</param>
/// <param name="CulturalBackground">文化的背景に関する説明。</param>
/// <param name="SkillsAndExpertise">スキル・専門性をまとめたテキスト。</param>
/// <param name="SkillsAndExpertiseList">スキル・専門性の構造化された一覧。</param>
/// <param name="HobbiesAndInterests">趣味・関心事をまとめたテキスト。</param>
/// <param name="HobbiesAndInterestsList">趣味・関心事の構造化された一覧。</param>
/// <param name="CareerGoalsAndAmbitions">キャリアの目標および志向。</param>
/// <param name="Sex">ペルソナの性別。</param>
/// <param name="Age">ペルソナの年齢。</param>
/// <param name="MaritalStatus">婚姻状況。</param>
/// <param name="EducationLevel">最終学歴。</param>
/// <param name="Occupation">現在の職業。</param>
/// <param name="Region">地域（国内外の広域区分）。</param>
/// <param name="Area">地域内のエリアまたはサブリージョン。</param>
/// <param name="Prefecture">都道府県名。</param>
/// <param name="Country">国名。</param>
public sealed record PersonaRecord(
    string Uuid,
    string? ProfessionalPersona,
    string? SportsPersona,
    string? ArtsPersona,
    string? TravelPersona,
    string? CulinaryPersona,
    string? Persona,
    string? CulturalBackground,
    string? SkillsAndExpertise,
    IReadOnlyList<string>? SkillsAndExpertiseList,
    string? HobbiesAndInterests,
    IReadOnlyList<string>? HobbiesAndInterestsList,
    string? CareerGoalsAndAmbitions,
    string? Sex,
    int? Age,
    string? MaritalStatus,
    string? EducationLevel,
    string? Occupation,
    string? Region,
    string? Area,
    string? Prefecture,
    string? Country
);
