using Resume.Core.Entities;

namespace Resume.Core.RepositoryContracts;

/// <summary>
/// Contrato para el repositorio de educación académica.
/// </summary>
public interface IAcademicEducationRepository
{
    /// <summary>
    /// Obtiene todas las entradas de educación académica asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <returns>Una colección de entradas de educación académica.</returns>
    Task<IEnumerable<AcademicEducation>> GetAcademicEducationsByProfessionalResumeId(Guid professionalResumeId);

    /// <summary>
    /// Reemplaza todas las entradas de educación académica asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <param name="academicEducations">Colección de entradas de educación académica a crear.</param>
    /// <returns>True si ambas operaciones fueron exitosas; de lo contrario, false.</returns>
    Task<bool> ReplaceAcademicEducations(Guid professionalResumeId, IEnumerable<AcademicEducation> academicEducations);
}