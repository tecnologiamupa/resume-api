using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.ServiceContracts;

/// <summary>
/// Contrato para el servicio de habilidades técnicas.
/// </summary>
public interface ITechnicalSkillService
{
    /// <summary>
    /// Obtiene todas las habilidades técnicas asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <returns>Una colección de habilidades técnicas.</returns>
    Task<BaseResponse<List<TechnicalSkillResponse>>> GetTechnicalSkillsByProfessionalResumeId(Guid professionalResumeId);

    /// <summary>
    /// Reemplaza todas las habilidades técnicas asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <param name="technicalSkills">Colección de habilidades técnicas a crear.</param>
    /// <returns>True si ambas operaciones fueron exitosas; de lo contrario, false.</returns>
    Task<BaseResponse<bool>> ReplaceTechnicalSkills(Guid professionalResumeId, List<TechnicalSkillCreateRequest> technicalSkills);
}