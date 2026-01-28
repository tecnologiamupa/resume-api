using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Resume.Core.Entities;

namespace Resume.Core.RepositoryContracts
{
    /// <summary>
    /// Contrato para el repositorio de habilidades profesionales.
    /// </summary>
    public interface IProfessionalSkillRepository
    {
        /// <summary>
        /// Obtiene todas las habilidades profesionales asociadas a un currículum profesional.
        /// </summary>
        /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
        /// <returns>Una colección de habilidades profesionales.</returns>
        Task<IEnumerable<ProfessionalSkill>> GetSkillsByProfessionalResumeId(Guid professionalResumeId);

        /// <summary>
        /// Elimina todas las habilidades profesionales asociadas a un currículum profesional y crea nuevas habilidades profesionales en una sola transacción.
        /// </summary>
        /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
        /// <param name="professionalSkills">Colección de habilidades profesionales a crear.</param>
        /// <returns>True si ambas operaciones fueron exitosas; de lo contrario, false.</returns>
        Task<bool> ReplaceProfessionalSkills(Guid professionalResumeId, IEnumerable<ProfessionalSkill> professionalSkills);
    }
}