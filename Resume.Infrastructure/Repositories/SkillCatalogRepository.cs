using Dapper;
using Resume.Core.Entities;
using Resume.Core.RepositoryContracts;
using Resume.Infrastructure.DbContexts;

namespace Resume.Infrastructure.Repositories;

internal class SkillCatalogRepository : ISkillCatalogRepository
{
    private readonly RecruitmentSharedDbContext _dbContext;

    public SkillCatalogRepository(RecruitmentSharedDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Obtiene una lista de todas las habilidades disponibles en el catálogo.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El valor de retorno contiene una colección de objetos <see cref="SkillCatalog"/>.
    /// </returns>
    public async Task<IEnumerable<SkillCatalog?>> GetSkillsCatalog()
    {
        string query = @"
            SELECT * 
            FROM `Skill` 
            ORDER BY 
                CASE WHEN Name = 'Otros' THEN 1 ELSE 0 END, 
                Name ASC";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryAsync<SkillCatalog>(query);
        }
    }

    /// <summary>
    /// Obtiene una habilidad específica del catálogo por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único de la habilidad.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El valor de retorno contiene un objeto <see cref="SkillCatalog"/> si se encuentra; de lo contrario, <c>null</c>.
    /// </returns>
    public async Task<SkillCatalog?> GetSkillCatalogById(int id)
    {
        string query = "SELECT * FROM `Skill` WHERE Id = @Id";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryFirstOrDefaultAsync<SkillCatalog>(query, new { Id = id });
        }
    }
}