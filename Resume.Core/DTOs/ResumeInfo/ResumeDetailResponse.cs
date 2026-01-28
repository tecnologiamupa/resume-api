namespace Resume.Core.DTOs;

/// <summary>
/// Representa la respuesta detallada de un currículum, incluyendo información personal, profesional y otros detalles relevantes.
/// </summary>
public class ResumeDetailResponse
{
    public Guid Id { get; set; }
    public string LinkedIn { get; set; }
    public string PortfolioUrl { get; set; }
    public string Summary { get; set; }
    public DateTime? CreatedDate { get; set; }
    public PersonalInfoResponse? PersonalInfo { get; set; } = null;
    //public List<string>? Languages { get; set; }
    public List<LanguageResponse>? Languages { get; set; }
    public ProfessionalInfoResponse? ProfessionalInfo { get; set; } = null;
}
