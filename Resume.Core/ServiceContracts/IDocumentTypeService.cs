using Resume.Core.DTOs;

namespace Resume.Core.ServiceContracts;

/// <summary>
/// Define el contrato para los servicios relacionados con los tipos de documentos.
/// </summary>
public interface IDocumentTypeService
{
    /// <summary>
    /// Obtiene la lista de tipos de documentos.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. 
    /// El resultado contiene una respuesta base con una lista de objetos <see cref="DocumentTypeResponse"/>.
    /// </returns>
    Task<BaseResponse<List<DocumentTypeResponse?>>> GetDocumentTypes();

    /// <summary>
    /// Obtiene un tipo de documento por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del tipo de documento.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. 
    /// El resultado contiene una respuesta base con un objeto <see cref="DocumentTypeResponse"/>.
    /// </returns>
    Task<BaseResponse<DocumentTypeResponse?>> GetDocumentTypeById(int id);
}