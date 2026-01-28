using Resume.Core.Entities;

namespace Resume.Core.DTOs;

public class ResumeWithVacancy : AuditableEntity
{
    public Guid Id { get; set; }
    public int ResumeTypeId { get; set; }
    public Guid? PersonalInfoId { get; set; }
    public string LinkedIn { get; set; }
    public string PortfolioUrl { get; set; }
    public string Summary { get; set; }
    public string VacancyName { get; set; }
    public PersonalInfo? PersonalInfo { get; set; } = null;
}
