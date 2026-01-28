namespace Resume.Core.DTOs;

/// <summary>
/// Representa la respuesta de un currículum deportivo.
/// </summary>
public class SportsResumeResponse
{
    public Guid Id { get; set; }
    public Guid? ResumeId { get; set; }
    public string? SportsSummary { get; set; }
}
