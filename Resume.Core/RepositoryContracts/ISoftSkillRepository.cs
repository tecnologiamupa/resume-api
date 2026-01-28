using Resume.Core.Entities;

namespace Resume.Core.RepositoryContracts;

/// <summary>
/// Contrato para el repositorio de habilidades blandas.
/// </summary>
public interface ISoftSkillRepository
{
    /// <summary>
    /// Obtiene todas las habilidades blandas asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <returns>Una colección de habilidades blandas.</returns>
    Task<IEnumerable<SoftSkill>> GetSoftSkillsByProfessionalResumeId(Guid professionalResumeId);

    /// <summary>
    /// Elimina todas las habilidades blandas asociadas a un currículum profesional y crea nuevas habilidades blandas en una sola transacción.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <param name="softSkills">Colección de habilidades blandas a crear.</param>
    /// <returns>True si ambas operaciones fueron exitosas; de lo contrario, false.</returns>
    Task<bool> ReplaceSoftSkills(Guid professionalResumeId, IEnumerable<SoftSkill> softSkills);
}