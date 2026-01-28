using Resume.Core.DTOs;

namespace Resume.Core.ServiceContracts;

public interface ILanguageCatalogService
{
    Task<BaseResponse<List<LanguageCatalogResponse?>>> GetLanguagesCatalog();
    Task<BaseResponse<LanguageCatalogResponse?>> GetLanguageCatalogById(int id);
}
