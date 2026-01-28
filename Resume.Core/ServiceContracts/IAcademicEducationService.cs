using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.ServiceContracts;

/// <summary>
/// Contrato para el servicio de educación académica.
/// </summary>
public interface IAcademicEducationService
{
    /// <summary>
    /// Obtiene todas las entradas de educación académica asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <returns>Una colección de entradas de educación académica.</returns>
    Task<BaseResponse<List<AcademicEducationResponse>>> GetAcademicEducationsByProfessionalResumeId(Guid professionalResumeId);

    /// <summary>
    /// Reemplaza todas las entradas de educación académica asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <param name="academicEducations">Colección de entradas de educación académica a crear.</param>
    /// <returns>True si ambas operaciones fueron exitosas; de lo contrario, false.</returns>
    Task<BaseResponse<bool>> ReplaceAcademicEducations(Guid professionalResumeId, List<AcademicEducationCreateRequest> academicEducations);
}