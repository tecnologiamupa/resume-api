namespace Resume.Core.Entities;

public class ProfessionalSkill
{
    public int Id { get; set; }
    public Guid ProfessionalResumeId { get; set; }
    public int? SkillId { get; set; }
    public string? Name { get; set; }
}
