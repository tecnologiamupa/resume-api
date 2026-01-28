namespace Resume.Core.DTOs;

public class ResumeWithVacancyResponse
{
    public Guid Id { get; set; }
    public string LinkedIn { get; set; }
    public string PortfolioUrl { get; set; }
    public string Summary { get; set; }
    public DateTime? CreatedDate { get; set; }
    public PersonalInfoResponse? PersonalInfo { get; set; } = null;
    public string VacancyName { get; set; }
}
