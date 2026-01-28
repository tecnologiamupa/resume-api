using Dapper;
using Resume.Core.Entities;
using Resume.Core.RepositoryContracts;
using Resume.Infrastructure.DbContexts;

namespace Resume.Infrastructure.Repositories;

/// <summary>
/// Repositorio para interactuar con los datos de los tipos de documentos.
/// </summary>
internal class DocumentTypeRepository : IDocumentTypeRepository
{
    private readonly ResumeDbContext _dbContext;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DocumentTypeRepository"/>.
    /// </summary>
    /// <param name="dbContext">El contexto de base de datos utilizado para acceder a los datos.</param>
    public DocumentTypeRepository(ResumeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Obtiene una lista de todos los tipos de documentos disponibles.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El valor de retorno contiene una colección de objetos <see cref="DocumentType"/>.
    /// </returns>
    public async Task<IEnumerable<DocumentType?>> GetDocumentTypes()
    {
        string query = @"
            SELECT * 
            FROM `DocumentType` 
            ORDER BY 
                CASE WHEN Name = 'Otros' THEN 1 ELSE 0 END, 
                Name ASC";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryAsync<DocumentType>(query);
        }
    }

    /// <summary>
    /// Obtiene un tipo de documento específico por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del tipo de documento.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El valor de retorno contiene un objeto <see cref="DocumentType"/> si se encuentra; de lo contrario, <c>null</c>.
    /// </returns>
    public async Task<DocumentType?> GetDocumentTypeById(int id)
    {
        string query = "SELECT * FROM `DocumentType` WHERE Id = @Id";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryFirstOrDefaultAsync<DocumentType>(query, new { Id = id });
        }
    }
}