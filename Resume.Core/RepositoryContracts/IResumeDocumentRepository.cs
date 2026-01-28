using Resume.Core.Entities;

namespace Resume.Core.RepositoryContracts;

/// <summary>
/// Define un contrato para las operaciones relacionadas con los documentos de currículum.
/// </summary>
public interface IResumeDocumentRepository
{
    /// <summary>
    /// Obtiene una colección de documentos asociados a un currículum específico.
    /// </summary>
    /// <param name="resumeId">El identificador único del currículum.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado contiene una colección de documentos de currículum.</returns>
    Task<IEnumerable<ResumeDocument?>> GetResumeDocumentsByResumeId(Guid resumeId);

    /// <summary>
    /// Obtiene un documento de currículum por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del documento.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado contiene el documento de currículum, o null si no se encuentra.</returns>
    Task<ResumeDocument?> GetResumeDocumentById(Guid id);

    /// <summary>
    /// Crea un nuevo documento de currículum.
    /// </summary>
    /// <param name="document">El documento de currículum a crear.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado contiene el documento de currículum creado.</returns>
    Task<ResumeDocument> CreateResumeDocument(ResumeDocument document);

    /// <summary>
    /// Elimina un documento de currículum por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del documento a eliminar.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado indica si la operación fue exitosa.</returns>
    Task<bool> DeleteResumeDocument(Guid id);
}
