namespace Resume.Core.DTOs;

/// <summary>
/// Representa una solicitud para manejar información profesional, incluyendo experiencias laborales y habilidades blandas.
/// </summary>
public class ProfessionalInfoRequest
{
    public List<WorkExperienceRequest>? WorkExperiences { get; set; }
    public List<AcademicEducationRequest>? AcademicEducations { get; set; }
    public List<string>? SoftSkills { get; set; }
    public List<string>? TechnicalSkills { get; set; }
    public List<ProfessionalSkillCreateRequest>? Skills { get; set; }
    public InternshipRequest? Internship { get; set; }
    public InadehRequest? Inadeh { get; set; }
}
