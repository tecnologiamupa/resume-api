using Resume.Core.Entities;

namespace Resume.Core.RepositoryContracts;

public interface ISkillCatalogRepository
{
    Task<IEnumerable<SkillCatalog?>> GetSkillsCatalog();
    Task<SkillCatalog?> GetSkillCatalogById(int id);
}
