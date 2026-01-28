using Resume.Core.Entities;

namespace Resume.Core.RepositoryContracts;

/// <summary>
/// Contrato para el repositorio de idiomas personales.
/// </summary>
public interface ILanguageRepository
{
    /// <summary>
    /// Obtiene todos los idiomas asociados a una información personal.
    /// </summary>
    /// <param name="personalInfoId">Identificador de la información personal.</param>
    /// <returns>Una colección de idiomas personales.</returns>
    Task<IEnumerable<PersonalLanguage>> GetLanguagesByPersonalInfoId(Guid personalInfoId);

    /// <summary>
    /// Elimina todos los idiomas asociados a una información personal y crea nuevos idiomas en una sola transacción.
    /// </summary>
    /// <param name="personalInfoId">Identificador de la información personal.</param>
    /// <param name="languages">Colección de idiomas a crear.</param>
    /// <returns>True si ambas operaciones fueron exitosas; de lo contrario, false.</returns>
    Task<bool> ReplaceLanguages(Guid personalInfoId, IEnumerable<PersonalLanguage> languages);
}