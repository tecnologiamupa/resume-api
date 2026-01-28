namespace Resume.Core.DTOs;

/// <summary>
/// Representa una solicitud para crear una habilidad técnica asociada con un currículum profesional.
/// </summary>
public class TechnicalSkillCreateRequest
{
    public Guid ProfessionalResumeId { get; set; }
    public string? Skill { get; set; }
}