namespace Resume.Core.DTOs;

/// <summary>
/// Representa una solicitud para actualizar un currículum deportivo.
/// </summary>
public class SportsResumeUpdateRequest
{
    public Guid? ResumeId { get; set; }
    public string? SportsSummary { get; set; }
}
