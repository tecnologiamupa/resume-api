namespace Resume.Core.DTOs;

/// <summary>
/// Representa una solicitud para crear un idioma asociado con una información personal.
/// </summary>
public class LanguageCreateRequest
{
    //public Guid PersonalInfoId { get; set; }
    public int? LanguageId { get; set; }
    public string? Name { get; set; }
}