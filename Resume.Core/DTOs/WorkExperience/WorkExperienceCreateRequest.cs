using System;

namespace Resume.Core.DTOs;

/// <summary>
/// Solicitud para crear una experiencia laboral.
/// </summary>
public class WorkExperienceCreateRequest
{
    public Guid ProfessionalResumeId { get; set; }
    public string? Company { get; set; }
    public string? Position { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? CurrentlyWorking { get; set; }
    public string? PositionDescription { get; set; }
}