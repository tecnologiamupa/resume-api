namespace Resume.Core.Entities;

/// <summary>
/// Representa un currículum deportivo asociado a un currículum general.
/// </summary>
public class SportsResume : AuditableEntity
{
    public Guid Id { get; set; }
    public Guid? ResumeId { get; set; }
    public string? SportsSummary { get; set; }
}
