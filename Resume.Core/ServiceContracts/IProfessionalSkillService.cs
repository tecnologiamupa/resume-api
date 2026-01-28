using Resume.Core.DTOs;

namespace Resume.Core.ServiceContracts;

/// <summary>
/// Contrato para el servicio de habilidades profesionales.
/// </summary>
public interface IProfessionalSkillService
{
    /// <summary>
    /// Obtiene todas las habilidades profesionales asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <returns>Una colección de habilidades profesionales.</returns>
    Task<BaseResponse<List<ProfessionalSkillResponse>>> GetSkillsByProfessionalResumeId(Guid professionalResumeId);

    /// <summary>
    /// Reemplaza todas las habilidades profesionales asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <param name="professionalSkills">Colección de habilidades profesionales a crear.</param>
    /// <returns>True si ambas operaciones fueron exitosas; de lo contrario, false.</returns>
    Task<BaseResponse<bool>> ReplaceProfessionalSkills(Guid professionalResumeId, List<ProfessionalSkillCreateRequest> professionalSkills);
}