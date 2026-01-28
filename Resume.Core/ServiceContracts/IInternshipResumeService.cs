using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.ServiceContracts;

/// <summary>
/// Define los contratos de servicio para gestionar currículums de prácticas.
/// </summary>
public interface IInternshipResumeService
{
    /// <summary>
    /// Obtiene un currículum de prácticas por su identificador único.
    /// </summary>
    /// <param name="id">Identificador único del currículum de prácticas.</param>
    /// <returns>Una respuesta que contiene el currículum de prácticas o null si no se encuentra.</returns>
    Task<BaseResponse<InternshipResumeResponse?>> GetInternshipResumeById(Guid id);

    /// <summary>
    /// Obtiene un currículum de prácticas asociado a un currículum general por su identificador.
    /// </summary>
    /// <param name="resumeId">Identificador único del currículum general.</param>
    /// <returns>Una respuesta que contiene el currículum de prácticas o null si no se encuentra.</returns>
    Task<BaseResponse<InternshipResumeResponse?>> GetInternshipResumeByResumeId(Guid resumeId);

    /// <summary>
    /// Crea un nuevo currículum de prácticas.
    /// </summary>
    /// <param name="internshipResume">Datos necesarios para crear el currículum de prácticas.</param>
    /// <returns>Una respuesta que contiene el currículum de prácticas creado.</returns>
    Task<BaseResponse<InternshipResumeResponse>> CreateInternshipResume(InternshipResumeCreateRequest internshipResume);

    /// <summary>
    /// Actualiza un currículum de prácticas existente.
    /// </summary>
    /// <param name="id">Identificador único del currículum de prácticas a actualizar.</param>
    /// <param name="internshipResume">Datos actualizados del currículum de prácticas.</param>
    /// <returns>Una respuesta que indica si la actualización fue exitosa.</returns>
    Task<BaseResponse<bool>> UpdateInternshipResume(Guid id, InternshipResumeUpdateRequest internshipResume);

    /// <summary>
    /// Elimina un currículum de prácticas por su identificador único.
    /// </summary>
    /// <param name="id">Identificador único del currículum de prácticas a eliminar.</param>
    /// <returns>Una respuesta que indica si la eliminación fue exitosa.</returns>
    Task<BaseResponse<bool>> DeleteInternshipResume(Guid id);
}