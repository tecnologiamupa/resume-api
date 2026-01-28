using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.ServiceContracts;

/// <summary>
/// Contrato para el servicio de idiomas.
/// </summary>
public interface ILanguageService
{
    /// <summary>
    /// Obtiene todos los idiomas asociados a una información personal.
    /// </summary>
    /// <param name="personalInfoId">Identificador de la información personal.</param>
    /// <returns>Una colección de idiomas.</returns>
    Task<BaseResponse<List<LanguageResponse?>>> GetLanguagesByPersonalInfoId(Guid personalInfoId);

    /// <summary>
    /// Reemplaza todos los idiomas asociados a una información personal.
    /// </summary>
    /// <param name="personalInfoId">Identificador de la información personal.</param>
    /// <param name="languages">Colección de idiomas a crear.</param>
    /// <returns>True si ambas operaciones fueron exitosas; de lo contrario, false.</returns>
    Task<BaseResponse<bool>> ReplaceLanguages(Guid personalInfoId, List<LanguageCreateRequest> languages);
}