namespace Resume.Core.DTOs;

/// <summary>
/// Representa una solicitud para crear o actualizar una entrada de educación académica.
/// </summary>
public class AcademicEducationRequest
{
    public string? Institution { get; set; }
    public string? Degree { get; set; }
    public string? FieldOfStudy { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? CurrentlyStudying { get; set; }
    public string? AdditionalDescription { get; set; }
}