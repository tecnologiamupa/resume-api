using Resume.Core.Entities;

namespace Resume.Core.RepositoryContracts
{
    public interface ILanguageCatalogRepository
    {
        Task<IEnumerable<LanguageCatalog?>> GetLanguagesCatalog();
        Task<LanguageCatalog?> GetLanguageCatalogById(int id);
    }
}
