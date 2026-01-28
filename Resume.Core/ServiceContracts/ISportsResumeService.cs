using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.ServiceContracts;

/// <summary>
/// Define los contratos para las operaciones relacionadas con currículums deportivos.
/// </summary>
public interface ISportsResumeService
{
    /// <summary>
    /// Obtiene un currículum deportivo por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del currículum deportivo.</param>
    /// <returns>Una respuesta que contiene el currículum deportivo o null si no se encuentra.</returns>
    Task<BaseResponse<SportsResumeResponse?>> GetSportsResumeById(Guid id);

    /// <summary>
    /// Obtiene un currículum deportivo asociado a un currículum general por su identificador.
    /// </summary>
    /// <param name="resumeId">El identificador único del currículum general.</param>
    /// <returns>Una respuesta que contiene el currículum deportivo o null si no se encuentra.</returns>
    Task<BaseResponse<SportsResumeResponse?>> GetSportsResumeByResumeId(Guid resumeId);

    /// <summary>
    /// Crea un nuevo currículum deportivo.
    /// </summary>
    /// <param name="sportsResume">La solicitud con los datos necesarios para crear el currículum deportivo.</param>
    /// <returns>Una respuesta que contiene el currículum deportivo creado.</returns>
    Task<BaseResponse<SportsResumeResponse>> CreateSportsResume(SportsResumeCreateRequest sportsResume);

    /// <summary>
    /// Actualiza un currículum deportivo existente.
    /// </summary>
    /// <param name="id">El identificador único del currículum deportivo a actualizar.</param>
    /// <param name="sportsResume">La solicitud con los datos actualizados del currículum deportivo.</param>
    /// <returns>Una respuesta que indica si la actualización fue exitosa.</returns>
    Task<BaseResponse<bool>> UpdateSportsResume(Guid id, SportsResumeUpdateRequest sportsResume);

    /// <summary>
    /// Elimina un currículum deportivo por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del currículum deportivo a eliminar.</param>
    /// <returns>Una respuesta que indica si la eliminación fue exitosa.</returns>
    Task<BaseResponse<bool>> DeleteSportsResume(Guid id);
}