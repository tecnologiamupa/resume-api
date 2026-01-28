using Dapper;
using Resume.Core.Entities;
using Resume.Core.RepositoryContracts;
using Resume.Infrastructure.DbContexts;

namespace Resume.Infrastructure.Repositories;

/// <summary>
/// Repositorio para interactuar con los datos de los cursos de INADEH.
/// </summary>
internal class InadehCourseRepository : IInadehCourseRepository
{
    private readonly ResumeDbContext _dbContext;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="InadehCourseRepository"/>.
    /// </summary>
    /// <param name="dbContext">El contexto de base de datos utilizado para acceder a los datos.</param>
    public InadehCourseRepository(ResumeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Obtiene una lista de todos los cursos de INADEH disponibles.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El valor de retorno contiene una colección de objetos <see cref="InadehCourse"/>.
    /// </returns>
    public async Task<IEnumerable<InadehCourse?>> GetInadehCourses()
    {
        string query = "SELECT * FROM `InadehCourse`";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryAsync<InadehCourse>(query);
        }
    }

    /// <summary>
    /// Obtiene un curso de INADEH específico por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del curso.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El valor de retorno contiene un objeto <see cref="InadehCourse"/> si se encuentra; de lo contrario, <c>null</c>.
    /// </returns>
    public async Task<InadehCourse?> GetInadehCourseById(int id)
    {
        string query = "SELECT * FROM `InadehCourse` WHERE Id = @Id";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryFirstOrDefaultAsync<InadehCourse>(query, new { Id = id });
        }
    }
}
