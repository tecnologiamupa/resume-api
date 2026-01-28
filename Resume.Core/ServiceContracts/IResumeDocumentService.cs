using Resume.Core.DTOs;

namespace Resume.Core.ServiceContracts;

/// <summary>
/// Interfaz para gestionar documentos de currículum.
/// </summary>
public interface IResumeDocumentService
{
    /// <summary>
    /// Obtiene una lista de documentos asociados a un currículum específico.
    /// </summary>
    /// <param name="resumeId">El identificador único del currículum.</param>
    /// <returns>Una respuesta que contiene una lista de documentos del currículum.</returns>
    Task<BaseResponse<List<ResumeDocumentResponse?>>> GetResumeDocumentsByResumeId(Guid resumeId);

    /// <summary>
    /// Crea un nuevo documento asociado a un currículum.
    /// </summary>
    /// <param name="request">La solicitud que contiene los datos necesarios para crear el documento.</param>
    /// <returns>Una respuesta que contiene los detalles del documento creado.</returns>
    Task<BaseResponse<ResumeDocumentResponse?>> CreateResumeDocument(ResumeDocumentCreateRequest request);

    /// <summary>
    /// Elimina un documento de currículum por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del documento a eliminar.</param>
    /// <returns>Una respuesta que indica si la operación fue exitosa.</returns>
    Task<BaseResponse<bool>> DeleteResumeDocument(Guid id);
}
