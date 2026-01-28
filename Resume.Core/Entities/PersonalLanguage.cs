namespace Resume.Core.Entities;

/// <summary>
/// Representa un idioma asociado a la información personal.
/// </summary>
public class PersonalLanguage
{
    public int Id { get; set; }
    public Guid? PersonalInfoId { get; set; }
    public int? LanguageId { get; set; }
    public string? Name { get; set; }
}