namespace Resume.Core.Entities;

/// <summary>
/// Representa una entidad de educación académica asociada a un currículum profesional.
/// </summary>
public class AcademicEducation : AuditableEntity
{
    public int Id { get; set; }
    public Guid? ProfessionalResumeId { get; set; }
    public string? Institution { get; set; }
    public string? Degree { get; set; }
    public string? FieldOfStudy { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? CurrentlyStudying { get; set; }
    public string? AdditionalDescription { get; set; }
}