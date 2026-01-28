using Resume.Core.Entities;

namespace Resume.Core.RepositoryContracts;

/// <summary>
/// Define un contrato para las operaciones relacionadas con la información personal.
/// </summary>
public interface IPersonalInfoRepository
{
    /// <summary>
    /// Obtiene una lista de todas las informaciones personales.
    /// </summary>
    /// <returns>Una tarea que representa una colección de objetos <see cref="PersonalInfo"/>.</returns>
    Task<IEnumerable<PersonalInfo?>> GetPersonalInfos();

    /// <summary>
    /// Obtiene la información personal por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único de la información personal.</param>
    /// <returns>Una tarea que representa un objeto <see cref="PersonalInfo"/> o null si no se encuentra.</returns>
    Task<PersonalInfo?> GetPersonalInfoById(Guid id);

    /// <summary>
    /// Obtiene la información personal por su número de identidad.
    /// </summary>
    /// <param name="identityNumber">El número de identidad de la persona.</param>
    /// <returns>Una tarea que representa un objeto <see cref="PersonalInfo"/> o null si no se encuentra.</returns>
    Task<PersonalInfo?> GetPersonalInfoByIdentityNumber(string identityNumber);

    Task<PersonalInfo?> GetPersonalInfoByMobile(string mobile);

    /// <summary>
    /// Crea una nueva información personal.
    /// </summary>
    /// <param name="personalInfo">El objeto <see cref="PersonalInfo"/> que se va a crear.</param>
    /// <returns>Una tarea que representa el objeto <see cref="PersonalInfo"/> creado.</returns>
    Task<PersonalInfo> CreatePersonalInfo(PersonalInfo personalInfo);

    /// <summary>
    /// Actualiza una información personal existente.
    /// </summary>
    /// <param name="personalInfo">El objeto <see cref="PersonalInfo"/> con los datos actualizados.</param>
    /// <returns>Una tarea que representa un valor booleano indicando si la operación fue exitosa.</returns>
    Task<bool> UpdatePersonalInfo(PersonalInfo personalInfo);

    /// <summary>
    /// Actualiza la foto de perfil de una información personal existente.
    /// </summary>
    /// <param name="personalInfo">El objeto <see cref="PersonalInfo"/> con la URL de la foto de perfil actualizada.</param>
    /// <returns>Una tarea que representa un valor booleano indicando si la operación fue exitosa.</returns>
    Task<bool> UpdateProfilePhoto(PersonalInfo personalInfo);

    /// <summary>
    /// Elimina una información personal por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único de la información personal a eliminar.</param>
    /// <returns>Una tarea que representa un valor booleano indicando si la operación fue exitosa.</returns>
    Task<bool> DeletePersonalInfo(Guid id);

    /// <summary>
    /// Obtiene la información personal por número de identidad y móvil.
    /// </summary>
    /// <param name="identityNumber">El número de identidad de la persona.</param>
    /// <param name="mobile">El número de móvil de la persona.</param>
    /// <returns>Una tarea que representa un objeto <see cref="PersonalInfo"/> o null si no se encuentra.</returns>
    Task<PersonalInfo?> GetPersonalInfoByIdentityNumberAndMobile(string identityNumber, string phoneCountryCode, string mobile);
}
