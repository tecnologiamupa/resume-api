using Dapper;
using Resume.Core.Entities;
using Resume.Core.RepositoryContracts;
using Resume.Infrastructure.DbContexts;

namespace Resume.Infrastructure.Repositories;

/// <summary>
/// Repositorio para interactuar con los datos de los tipos de discapacidades.
/// </summary>
internal class DisabilityTypeRepository : IDisabilityTypeRepository
{
    private readonly ResumeDbContext _dbContext;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DisabilityTypeRepository"/>.
    /// </summary>
    /// <param name="dbContext">El contexto de base de datos utilizado para acceder a los datos.</param>
    public DisabilityTypeRepository(ResumeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Obtiene una lista de todos los tipos de discapacidades disponibles.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El valor de retorno contiene una colección de objetos <see cref="DisabilityType"/>.
    /// </returns>
    public async Task<IEnumerable<DisabilityType?>> GetDisabilityTypes()
    {
        string query = @"
            SELECT * 
            FROM `DisabilityType` 
            ORDER BY 
                CASE WHEN Name = 'Otros' THEN 1 ELSE 0 END, 
                Name ASC";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryAsync<DisabilityType>(query);
        }
    }

    /// <summary>
    /// Obtiene un tipo de discapacidad específico por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del tipo de discapacidad.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El valor de retorno contiene un objeto <see cref="DisabilityType"/> si se encuentra; de lo contrario, <c>null</c>.
    /// </returns>
    public async Task<DisabilityType?> GetDisabilityTypeById(int id)
    {
        string query = "SELECT * FROM `DisabilityType` WHERE Id = @Id";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryFirstOrDefaultAsync<DisabilityType>(query, new { Id = id });
        }
    }
}