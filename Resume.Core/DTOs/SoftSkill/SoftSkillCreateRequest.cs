namespace Resume.Core.DTOs;

/// <summary>
/// Representa una solicitud para crear una habilidad blanda asociada con un currículum profesional.
/// </summary>
public class SoftSkillCreateRequest
{
    public Guid ProfessionalResumeId { get; set; }
    public string? Skill { get; set; }
}