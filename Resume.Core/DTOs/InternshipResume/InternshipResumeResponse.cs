namespace Resume.Core.DTOs;

/// <summary>
/// Representa la respuesta de un currículum de prácticas.
/// </summary>
public class InternshipResumeResponse
{
    public Guid Id { get; set; }
    public Guid? ResumeId { get; set; }
    public string? CareerObjective { get; set; }
}
