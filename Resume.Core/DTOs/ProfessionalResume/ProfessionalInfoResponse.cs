namespace Resume.Core.DTOs;

/// <summary>
/// Representa la respuesta con la información profesional de un currículum.
/// </summary>
public class ProfessionalInfoResponse
{
    public List<WorkExperienceResponse>? WorkExperiences { get; set; }
    public List<AcademicEducationResponse>? AcademicEducations { get; set; }
    public List<string>? SoftSkills { get; set; }
    public List<string>? TechnicalSkills { get; set; }
    public List<ProfessionalSkillResponse>? Skills { get; set; }
    public InternshipResponse? Internship { get; set; }
    public InadehResponse? Inadeh { get; set; }
    public PlatziResponse? Platzi { get; set; }
}
