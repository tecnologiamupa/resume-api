using System;

namespace Resume.Core.Entities;

/// <summary>
/// Representa una habilidad blanda asociada a un currículum profesional.
/// </summary>
public class SoftSkill
{
    public int Id { get; set; }
    public Guid? ProfessionalResumeId { get; set; }
    public string? Skill { get; set; }
}