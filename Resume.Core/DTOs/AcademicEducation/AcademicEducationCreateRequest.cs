namespace Resume.Core.DTOs;

/// <summary>
/// Solicitud para crear una entrada de educación académica.
/// </summary>
public class AcademicEducationCreateRequest
{
    public Guid ProfessionalResumeId { get; set; }
    public string? Institution { get; set; }
    public string? Degree { get; set; }
    public string? FieldOfStudy { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? CurrentlyStudying { get; set; }
    public string? AdditionalDescription { get; set; }
}