namespace Resume.Core.Entities;

/// <summary>
/// Representa la información principal de un currículum.
/// </summary>
public class ResumeInfo : AuditableEntity
{
    public Guid Id { get; set; }
    public int ResumeTypeId { get; set; }
    public Guid? PersonalInfoId { get; set; }
    public string LinkedIn { get; set; }
    public string PortfolioUrl { get; set; }
    public string Summary { get; set; }
}
