namespace Resume.Core.DTOs;

/// <summary>
/// Representa la respuesta de un currículum con información básica.
/// </summary>
public class ResumeResponse
{
    public Guid Id { get; set; }
    public string LinkedIn { get; set; }
    public string PortfolioUrl { get; set; }
    public string Summary { get; set; }
    public DateTime? CreatedDate { get; set; }
    public PersonalInfoResponse? PersonalInfo { get; set; } = null;
    public ProfessionalInfoResponse? ProfessionalInfo { get; set; } = null;
}
