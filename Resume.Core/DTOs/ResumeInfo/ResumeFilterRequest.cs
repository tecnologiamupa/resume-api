namespace Resume.Core.DTOs;

public class ResumeFilterRequest
{
    public string? IdentityNumber { get; set; }
    public List<string>? IdentityNumbers { get; set; }
    public string? Mobile { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? SearchText { get; set; }
    public Guid? CompanyId { get; set; }
    public Guid? VacancyId { get; set; }
    public int? VacancyResumeStatusId { get; set; }
    public bool? HasDisability { get; set; }
    public bool? IsInternshipCandidate { get; set; }
    public bool? IsInadehCandidate { get; set; }
    public bool? IsPlatziAssigned { get; set; }
}