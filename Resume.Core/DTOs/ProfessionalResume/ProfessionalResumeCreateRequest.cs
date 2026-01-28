namespace Resume.Core.DTOs;

/// <summary>
/// Representa una solicitud para crear un currículum profesional.
/// </summary>
public class ProfessionalResumeCreateRequest
{
    public Guid? ResumeId { get; set; }
    public string? ProfessionalSummary { get; set; }
}
