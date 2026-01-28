using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.ServiceContracts;

/// <summary>
/// Contrato para el servicio de experiencias laborales.
/// </summary>
public interface IWorkExperienceService
{
    /// <summary>
    /// Obtiene todas las experiencias laborales asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <returns>Una colección de experiencias laborales.</returns>
    Task<BaseResponse<List<WorkExperienceResponse>>> GetWorkExperiencesByProfessionalResumeId(Guid professionalResumeId);

    /// <summary>
    /// Reemplaza todas las experiencias laborales asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <param name="workExperiences">Colección de experiencias laborales a crear.</param>
    /// <returns>True si ambas operaciones fueron exitosas; de lo contrario, false.</returns>
    Task<BaseResponse<bool>> ReplaceWorkExperiences(Guid professionalResumeId, List<WorkExperienceCreateRequest> workExperiences);
}