using Dapper;
using Resume.Core.Entities;
using Resume.Core.RepositoryContracts;
using Resume.Infrastructure.DbContexts;

namespace Resume.Infrastructure.Repositories;

/// <summary>
/// Repositorio para interactuar con los tipos de pasantías en la base de datos.
/// </summary>
internal class InternshipTypeRepository : IInternshipTypeRepository
{
    private readonly ResumeDbContext _dbContext;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="InternshipTypeRepository"/>.
    /// </summary>
    /// <param name="dbContext">El contexto de base de datos utilizado para acceder a la base de datos.</param>
    public InternshipTypeRepository(ResumeDbContext dbContext)
    {
        _dbContext = dbContext;
    }    

    /// <summary>
    /// Obtiene una colección de tipos de pasantías disponibles desde la base de datos.
    /// </summary>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado contiene una colección de tipos de pasantías.</returns>
    public async Task<IEnumerable<InternshipType?>> GetInternshipTypes()
    {
        string query = "SELECT * FROM `InternshipType`";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryAsync<InternshipType>(query);
        }
    }

    /// <summary>
    /// Obtiene un tipo de pasantía específico basado en su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del tipo de pasantía.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. 
    /// El resultado contiene un objeto <see cref="InternshipType"/> si se encuentra; de lo contrario, <c>null</c>.
    /// </returns>
    public async Task<InternshipType?> GetInternshipTypeById(int id)
    {
        string query = "SELECT * FROM `InternshipType` WHERE Id = @Id";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryFirstOrDefaultAsync<InternshipType>(query, new { Id = id });
        }
    }
}
