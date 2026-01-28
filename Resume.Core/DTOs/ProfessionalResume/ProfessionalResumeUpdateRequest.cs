namespace Resume.Core.DTOs;

/// <summary>
/// Representa una solicitud para actualizar un currículum profesional.
/// </summary>
public class ProfessionalResumeUpdateRequest
{
    public Guid? ResumeId { get; set; }
    public string? ProfessionalSummary { get; set; }
    public bool? IsInternshipCandidate { get; set; }
    public int? InternshipTypeId { get; set; }
    public bool? IsInadehCandidate { get; set; }
    public int? InadehCourseId { get; set; }
}
