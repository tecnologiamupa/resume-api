namespace Resume.Core.DTOs;

/// <summary>
/// Representa una solicitud para crear o actualizar una experiencia laboral.
/// </summary>
public class WorkExperienceRequest
{
    public string? Company { get; set; }
    public string? Position { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? CurrentlyWorking { get; set; }
    public string? PositionDescription { get; set; }
}