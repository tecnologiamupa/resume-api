using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.ServiceContracts;

/// <summary>
/// Contrato para el servicio de habilidades blandas.
/// </summary>
public interface ISoftSkillService
{
    /// <summary>
    /// Obtiene todas las habilidades blandas asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <returns>Una colección de habilidades blandas.</returns>
    Task<BaseResponse<List<SoftSkillResponse>>> GetSoftSkillsByProfessionalResumeId(Guid professionalResumeId);

    /// <summary>
    /// Reemplaza todas las habilidades blandas asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <param name="softSkills">Colección de habilidades blandas a crear.</param>
    /// <returns>True si ambas operaciones fueron exitosas; de lo contrario, false.</returns>
    Task<BaseResponse<bool>> ReplaceSoftSkills(Guid professionalResumeId, List<SoftSkillCreateRequest> softSkills);
}