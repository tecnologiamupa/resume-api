namespace Resume.Core.DTOs;

public class ProfessionalSkillResponse
{
    public string? Name { get; set; }
    public SkillCatalogResponse? SkillCatalog { get; set; } = null;
}
