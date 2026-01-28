namespace Resume.Core.DTOs;

/// <summary>
/// Representa una solicitud para crear un currículum deportivo.
/// </summary>
public class SportsResumeCreateRequest
{
    public Guid? ResumeId { get; set; }
    public string? SportsSummary { get; set; }
}
