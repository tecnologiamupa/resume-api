using Dapper;
using Resume.Core.Entities;
using Resume.Core.RepositoryContracts;
using Resume.Infrastructure.DbContexts;

namespace Resume.Infrastructure.Repositories;

/// <summary>
/// Repositorio para manejar las operaciones relacionadas con los documentos de currículum.
/// </summary>
internal class ResumeDocumentRepository : IResumeDocumentRepository
{
    private readonly ResumeDbContext _context;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ResumeDocumentRepository"/>.
    /// </summary>
    /// <param name="context">El contexto de base de datos utilizado para las operaciones.</param>
    public ResumeDocumentRepository(ResumeDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene una colección de documentos asociados a un currículum específico.
    /// </summary>
    /// <param name="resumeId">El identificador único del currículum.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado contiene una colección de documentos de currículum.</returns>
    public async Task<IEnumerable<ResumeDocument?>> GetResumeDocumentsByResumeId(Guid resumeId)
    {
        string query = "SELECT * FROM `ResumeDocument` WHERE ResumeId = @ResumeId";
        using (var connection = await _context.GetOpenConnectionAsync())
        {
            return await connection.QueryAsync<ResumeDocument>(query, new { ResumeId = resumeId });
        }
    }

    /// <summary>
    /// Obtiene un documento de currículum por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del documento.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado contiene el documento de currículum, o null si no se encuentra.</returns>
    public async Task<ResumeDocument?> GetResumeDocumentById(Guid id)
    {
        string query = "SELECT * FROM `ResumeDocument` WHERE Id = @Id";
        using (var connection = await _context.GetOpenConnectionAsync())
        {
            return await connection.QueryFirstOrDefaultAsync<ResumeDocument>(query, new { Id = id });
        }
    }

    /// <summary>
    /// Crea un nuevo documento de currículum.
    /// </summary>
    /// <param name="document">El documento de currículum a crear.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado contiene el documento de currículum creado.</returns>
    /// <exception cref="Exception">Se lanza si no se puede crear el documento.</exception>
    public async Task<ResumeDocument> CreateResumeDocument(ResumeDocument document)
    {
        string query = @"
            INSERT INTO `ResumeDocument` (
                Id, ResumeId, DocumentUrl, Title, DocumentTypeId, CreatedDate, CreatedBy
            ) VALUES (
                @Id, @ResumeId, @DocumentUrl, @Title, @DocumentTypeId, @CreatedDate, @CreatedBy
            );
        ";

        using (var connection = await _context.GetOpenConnectionAsync())
        {
            int rowCountAffected = await connection.ExecuteAsync(query, document);
            if (rowCountAffected > 0)
            {
                return document;
            }
            else
            {
                throw new Exception("No se pudo crear el documento del currículum.");
            }
        }
    }

    /// <summary>
    /// Elimina un documento de currículum por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del documento a eliminar.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado indica si la operación fue exitosa.</returns>
    public async Task<bool> DeleteResumeDocument(Guid id)
    {
        string query = "DELETE FROM `ResumeDocument` WHERE Id = @Id";
        using (var connection = await _context.GetOpenConnectionAsync())
        {
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }
    }
}