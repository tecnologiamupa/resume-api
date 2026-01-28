using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.ServiceContracts;

/// <summary>
/// Define los contratos para las operaciones relacionadas con currículums profesionales.
/// </summary>
public interface IProfessionalResumeService
{
    /// <summary>
    /// Obtiene un currículum profesional por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del currículum profesional.</param>
    /// <returns>Una respuesta que contiene el currículum profesional o null si no se encuentra.</returns>
    Task<BaseResponse<ProfessionalResumeResponse?>> GetProfessionalResumeById(Guid id);

    /// <summary>
    /// Obtiene un currículum profesional asociado a un currículum general por su identificador.
    /// </summary>
    /// <param name="resumeId">El identificador único del currículum general.</param>
    /// <returns>Una respuesta que contiene el currículum profesional o null si no se encuentra.</returns>
    Task<BaseResponse<ProfessionalResumeResponse?>> GetProfessionalResumeByResumeId(Guid resumeId);

    /// <summary>
    /// Crea un nuevo currículum profesional.
    /// </summary>
    /// <param name="professionalResume">La solicitud con los datos necesarios para crear el currículum profesional.</param>
    /// <returns>Una respuesta que contiene el currículum profesional creado.</returns>
    Task<BaseResponse<ProfessionalResumeResponse>> CreateProfessionalResume(ProfessionalResumeCreateRequest professionalResume);

    /// <summary>
    /// Actualiza un currículum profesional existente.
    /// </summary>
    /// <param name="id">El identificador único del currículum profesional a actualizar.</param>
    /// <param name="professionalResume">La solicitud con los datos actualizados del currículum profesional.</param>
    /// <returns>Una respuesta que indica si la actualización fue exitosa.</returns>
    Task<BaseResponse<bool>> UpdateProfessionalResume(Guid id, ProfessionalResumeUpdateRequest professionalResume);

    /// <summary>
    /// Actualiza en lote los campos Platzi (IsPlatziAssigned, PlatziCompanyUserId, PlatziUserId) de una lista de currículums profesionales, basado en ResumeId.
    /// </summary>
    /// <param name="requests">Lista de DTOs con los datos a actualizar.</param>
    /// <returns>Una respuesta que indica si la actualización fue exitosa.</returns>
    Task<BaseResponse<bool>> UpdatePlatziFieldsByResumeListAsync(IEnumerable<ProfessionalResumePlatziUpdateRequest> requests);

    /// <summary>
    /// Elimina un currículum profesional por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del currículum profesional a eliminar.</param>
    /// <returns>Una respuesta que indica si la eliminación fue exitosa.</returns>
    Task<BaseResponse<bool>> DeleteProfessionalResume(Guid id);
}