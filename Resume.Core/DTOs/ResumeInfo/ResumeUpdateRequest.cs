namespace Resume.Core.DTOs;

/// <summary>
/// Representa una solicitud para actualizar la información de un currículum.
/// </summary>
public class ResumeUpdateRequest
{
    public string? LinkedIn { get; set; }
    public string? PortfolioUrl { get; set; }
    public string? Summary { get; set; }
    public PersonalInfoCreateRequest? PersonalInfo { get; set; }
    //public List<string>? Languages { get; set; }
    public List<LanguageCreateRequest>? Languages { get; set; }
    public ProfessionalInfoRequest? ProfessionalInfo { get; set; }
    public SportsInfoRequest? SportsInfo { get; set; }
    public InternshipInfoRequest? InternshipInfo { get; set; }
}
