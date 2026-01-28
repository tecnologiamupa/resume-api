using Resume.Core.Entities;

namespace Resume.Core.RepositoryContracts;

/// <summary>
/// Contrato para el repositorio de habilidades técnicas.
/// </summary>
public interface ITechnicalSkillRepository
{
    /// <summary>
    /// Obtiene todas las habilidades técnicas asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <returns>Una colección de habilidades técnicas.</returns>
    Task<IEnumerable<TechnicalSkill>> GetTechnicalSkillsByProfessionalResumeId(Guid professionalResumeId);

    /// <summary>
    /// Elimina todas las habilidades técnicas asociadas a un currículum profesional y crea nuevas habilidades técnicas en una sola transacción.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <param name="technicalSkills">Colección de habilidades técnicas a crear.</param>
    /// <returns>True si ambas operaciones fueron exitosas; de lo contrario, false.</returns>
    Task<bool> ReplaceTechnicalSkills(Guid professionalResumeId, IEnumerable<TechnicalSkill> technicalSkills);
}