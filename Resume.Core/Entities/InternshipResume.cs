namespace Resume.Core.Entities;

/// <summary>
/// Representa un currículum de prácticas que incluye información relevante
/// como el objetivo profesional y la referencia a un currículum general.
/// </summary>
public class InternshipResume : AuditableEntity
{
    public Guid Id { get; set; }
    public Guid? ResumeId { get; set; }
    public string? CareerObjective { get; set; }
}
