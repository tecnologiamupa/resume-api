using Resume.Core.DTOs;
using Resume.Core.DTOs.ResumeInfo;
using Sigueme.Core.DTOs;

namespace Resume.Core.ServiceContracts;

/// <summary>
/// Define los contratos de servicio para la gestión de currículums.
/// </summary>
public interface IResumeService
{
    /// <summary>
    /// Obtiene la lista de currículums.
    /// </summary>
    /// <returns>Una respuesta con la lista de currículums.</returns>
    Task<BaseResponse<List<ResumeResponse?>>> GetResumes();

    Task<PaginatedResponse<List<ResumeResponse?>>> GetPagedResumes(int pageNumber, int pageSize);

    Task<PaginatedResponse<List<ResumeResponse?>>> GetPagedResumesByFilter(ResumeFilterRequest filter, int pageNumber, int pageSize);

    Task<PaginatedResponse<List<ResumeResponse?>>> GetCompletedResumesPaged(int pageNumber, int pageSize);

    Task<PaginatedResponse<List<ResumeResponse?>>> GetIncompleteResumesPaged(int pageNumber, int pageSize);

    Task<BaseResponse<List<ResumeWithVacancyResponse?>>> GetResumesByCompany(Guid companyId);

    /// <summary>
    /// Obtiene el detalle de un currículum por su identificador.
    /// </summary>
    /// <param name="id">Identificador del currículum.</param>
    /// <returns>Una respuesta con el detalle del currículum.</returns>
    Task<BaseResponse<ResumeDetailResponse?>> GetResumeById(Guid id);

    Task<BaseResponse<ResumeDetailResponse?>> GetMyResume();

    /// <summary>
    /// Crea un nuevo currículum.
    /// </summary>
    /// <param name="resume">Datos para crear el currículum.</param>
    /// <returns>Una respuesta con el currículum creado.</returns>
    Task<BaseResponse<ResumeResponse>> CreateResume(ResumeCreateRequest resume);

    /// <summary>
    /// Actualiza un currículum existente.
    /// </summary>
    /// <param name="id">Identificador del currículum.</param>
    /// <param name="resume">Datos para actualizar el currículum.</param>
    /// <returns>Una respuesta indicando si la actualización fue exitosa.</returns>
    Task<BaseResponse<ResumeResponse>> UpdateResume(Guid id, ResumeUpdateRequest resume);

    /// <summary>
    /// Elimina un currículum por su identificador.
    /// </summary>
    /// <param name="id">Identificador del currículum.</param>
    /// <returns>Una respuesta indicando si la eliminación fue exitosa.</returns>
    Task<BaseResponse<bool>> DeleteResume(Guid id);

    /// <summary>
    /// Obtiene el detalle de un currículum por datos de login (identityNumber, mobile, etc).
    /// </summary>
    /// <param name="request">Datos de login para buscar el currículum.</param>
    /// <returns>Una respuesta con el detalle del currículum.</returns>
    Task<BaseResponse<ResumeDetailResponse?>> GetResumeByLoginRequest(ResumeLoginRequest request);
}
