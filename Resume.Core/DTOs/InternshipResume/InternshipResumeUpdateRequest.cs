namespace Resume.Core.DTOs;

/// <summary>
/// Solicitud para actualizar un currículum de prácticas.
/// </summary>
public class InternshipResumeUpdateRequest
{
    public Guid? ResumeId { get; set; }
    public string? CareerObjective { get; set; }
}
