namespace Resume.Core.DTOs;

/// <summary>
/// Solicitud para crear un currículum de prácticas.
/// </summary>
public class InternshipResumeCreateRequest
{
    public Guid? ResumeId { get; set; }
    public string? CareerObjective { get; set; }
}
