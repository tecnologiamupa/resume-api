using Resume.Core.Entities;

namespace Resume.Core.RepositoryContracts;

/// <summary>
/// Define un contrato para las operaciones relacionadas con los tipos de documentos en el sistema.
/// </summary>
public interface IDocumentTypeRepository
{
    /// <summary>
    /// Obtiene una lista de todos los tipos de documentos disponibles.
    /// </summary>
    /// <returns>Una tarea que representa una colección de tipos de documentos.</returns>
    Task<IEnumerable<DocumentType?>> GetDocumentTypes();

    /// <summary>
    /// Obtiene un tipo de documento específico por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del tipo de documento.</param>
    /// <returns>Una tarea que representa el tipo de documento correspondiente o <c>null</c> si no se encuentra.</returns>
    Task<DocumentType?> GetDocumentTypeById(int id);
}
