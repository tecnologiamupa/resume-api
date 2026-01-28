namespace Resume.Core.DTOs;

/// <summary>
/// Representa la respuesta de un currículum profesional.
/// </summary>
public class ProfessionalResumeResponse
{
    public Guid Id { get; set; }
    public Guid? ResumeId { get; set; }
    public string? ProfessionalSummary { get; set; }
    public InternshipResponse? Internship { get; set; } = null;
    public InadehResponse? Inadeh { get; set; } = null;
    public PlatziResponse? Platzi { get; set; } = null;
}
