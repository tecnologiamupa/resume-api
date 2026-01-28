namespace Resume.Core.Entities;

/// <summary>
/// Representa un currículum profesional que incluye un resumen profesional y está asociado a un currículum específico.
/// Hereda de <see cref="AuditableEntity"/> para incluir propiedades de auditoría.
/// </summary>
public class ProfessionalResume : AuditableEntity
{
    public Guid Id { get; set; }
    public Guid? ResumeId { get; set; }
    public string? ProfessionalSummary { get; set; }
    public bool? IsInternshipCandidate { get; set; }
    public int? InternshipTypeId { get; set; }
    public bool? IsInadehCandidate { get; set; }
    public int? InadehCourseId { get; set; }
    public bool? IsPlatziAssigned { get; set; }
    public string? PlatziCompanyUserId { get; set; }
    public string? PlatziUserId { get; set; }
}
