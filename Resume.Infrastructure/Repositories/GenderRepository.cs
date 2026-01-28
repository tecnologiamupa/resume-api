using Dapper;
using Resume.Core.Entities;
using Resume.Core.RepositoryContracts;
using Resume.Infrastructure.DbContexts;

namespace Resume.Infrastructure.Repositories;

/// <summary>
/// Repositorio para la gestión de entidades de tipo <see cref="Gender"/>.
/// </summary>
internal class GenderRepository : IGenderRepository
{
    private readonly ResumeDbContext _dbContext;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="GenderRepository"/>.
    /// </summary>
    /// <param name="dbContext">El contexto de base de datos utilizado para acceder a la base de datos.</param>
    public GenderRepository(ResumeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Obtiene una lista de todos los géneros disponibles ordenados alfabéticamente por nombre.
    /// </summary>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado contiene una colección de objetos <see cref="Gender"/> o nulos.</returns>
    public async Task<IEnumerable<Gender?>> GetGenders()
    {
        string query = @"
            SELECT * 
            FROM `Gender` 
            ORDER BY Name ASC";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryAsync<Gender>(query);
        }
    }

    /// <summary>
    /// Obtiene un género específico por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del género.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado contiene un objeto <see cref="Gender"/> o nulo si no se encuentra.</returns>
    public async Task<Gender?> GetGenderById(int id)
    {
        string query = "SELECT * FROM `Gender` WHERE Id = @Id";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryFirstOrDefaultAsync<Gender>(query, new { Id = id });
        }
    }
}