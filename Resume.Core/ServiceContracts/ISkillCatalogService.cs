using Resume.Core.DTOs;

namespace Resume.Core.ServiceContracts;

public interface ISkillCatalogService
{
    Task<BaseResponse<List<SkillCatalogResponse?>>> GetSkillsCatalog();
    Task<BaseResponse<SkillCatalogResponse?>> GetSkillCatalogById(int id);
}
