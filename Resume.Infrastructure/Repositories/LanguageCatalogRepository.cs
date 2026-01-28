using Dapper;
using Resume.Core.Entities;
using Resume.Core.RepositoryContracts;
using Resume.Infrastructure.DbContexts;

namespace Resume.Infrastructure.Repositories;

internal class LanguageCatalogRepository : ILanguageCatalogRepository
{
    private readonly RecruitmentSharedDbContext _dbContext;

    public LanguageCatalogRepository(RecruitmentSharedDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Obtiene una lista de todos los idiomas disponibles en el catálogo.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El valor de retorno contiene una colección de objetos <see cref="LanguageCatalog"/>.
    /// </returns>
    public async Task<IEnumerable<LanguageCatalog?>> GetLanguagesCatalog()
    {
        string query = @"
            SELECT * 
            FROM `Languages` 
            ORDER BY 
                CASE WHEN Name = 'Otros' THEN 1 ELSE 0 END, 
                Name ASC";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryAsync<LanguageCatalog>(query);
        }
    }

    /// <summary>
    /// Obtiene un idioma específico del catálogo por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del idioma.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El valor de retorno contiene un objeto <see cref="LanguageCatalog"/> si se encuentra; de lo contrario, <c>null</c>.
    /// </returns>
    public async Task<LanguageCatalog?> GetLanguageCatalogById(int id)
    {
        string query = "SELECT * FROM `Languages` WHERE Id = @Id";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryFirstOrDefaultAsync<LanguageCatalog>(query, new { Id = id });
        }
    }
}