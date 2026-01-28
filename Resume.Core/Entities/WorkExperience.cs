using System;

namespace Resume.Core.Entities;

/// <summary>
/// Representa una experiencia laboral asociada a un currículum profesional.
/// </summary>
public class WorkExperience : AuditableEntity
{
    public int Id { get; set; }
    public Guid? ProfessionalResumeId { get; set; }
    public string? Company { get; set; }
    public string? Position { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? CurrentlyWorking { get; set; }
    public string? PositionDescription { get; set; }
}