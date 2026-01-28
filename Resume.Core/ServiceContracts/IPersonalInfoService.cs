using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.ServiceContracts;

/// <summary>
/// Define los contratos de servicio para gestionar la información personal de los usuarios.
/// </summary>
public interface IPersonalInfoService
{
    /// <summary>
    /// Obtiene una lista de todas las informaciones personales.
    /// </summary>
    /// <returns>Una respuesta que contiene una lista de <see cref="PersonalInfoResponse"/> o un error.</returns>
    Task<BaseResponse<List<PersonalInfoResponse?>>> GetPersonalInfos();

    /// <summary>
    /// Obtiene la información personal de un usuario por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del usuario.</param>
    /// <returns>Una respuesta que contiene la información personal del usuario o un error.</returns>
    Task<BaseResponse<PersonalInfoResponse?>> GetPersonalInfoById(Guid id);

    /// <summary>
    /// Obtiene la información personal de un usuario por su número de identidad.
    /// </summary>
    /// <param name="identityNumber">El número de identidad del usuario.</param>
    /// <returns>Una respuesta que contiene la información personal del usuario o un error.</returns>
    Task<BaseResponse<PersonalInfoResponse?>> GetPersonalInfoByIdentityNumber(string identityNumber);

    Task<BaseResponse<PersonalInfoResponse?>> GetPersonalInfoByMobile(string mobile);

    /// <summary>
    /// Crea una nueva información personal para un usuario.
    /// </summary>
    /// <param name="personalInfo">Los datos de la información personal a crear.</param>
    /// <returns>Una respuesta que contiene la información personal creada o un error.</returns>
    Task<BaseResponse<PersonalInfoResponse>> CreatePersonalInfo(PersonalInfoCreateRequest personalInfo);

    /// <summary>
    /// Actualiza la información personal de un usuario existente.
    /// </summary>
    /// <param name="id">El identificador único del usuario.</param>
    /// <param name="personalInfo">Los datos actualizados de la información personal.</param>
    /// <returns>Una respuesta que indica si la actualización fue exitosa o un error.</returns>
    Task<BaseResponse<bool>> UpdatePersonalInfo(Guid id, PersonalInfoUpdateRequest personalInfo);

    /// <summary>
    /// Elimina la información personal de un usuario por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del usuario.</param>
    /// <returns>Una respuesta que indica si la eliminación fue exitosa o un error.</returns>
    Task<BaseResponse<bool>> DeletePersonalInfo(Guid id);

    /// <summary>
    /// Obtiene la información personal de un usuario por número de identidad y móvil.
    /// </summary>
    /// <param name="identityNumber">El número de identidad del usuario.</param>
    /// <param name="mobile">El número de móvil del usuario.</param>
    /// <returns>Una respuesta que contiene la información personal del usuario o un error.</returns>
    Task<BaseResponse<PersonalInfoResponse?>> GetPersonalInfoByIdentityNumberAndMobile(string identityNumber, string phoneCountryCode, string mobile);
}
