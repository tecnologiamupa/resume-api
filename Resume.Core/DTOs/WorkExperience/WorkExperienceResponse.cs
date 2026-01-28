using System;

namespace Resume.Core.DTOs;

/// <summary>
/// Representa la respuesta con la información de una experiencia laboral.
/// </summary>
public class WorkExperienceResponse
{
    public string? Company { get; set; }
    public string? Position { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? CurrentlyWorking { get; set; }
    public string? PositionDescription { get; set; }
}