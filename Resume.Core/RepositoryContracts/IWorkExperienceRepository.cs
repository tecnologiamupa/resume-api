using Resume.Core.Entities;

namespace Resume.Core.RepositoryContracts;

/// <summary>
/// Contrato para el repositorio de experiencias laborales.
/// </summary>
public interface IWorkExperienceRepository
{
    /// <summary>
    /// Obtiene todas las experiencias laborales asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador único del currículum profesional.</param>
    /// <returns>Una tarea que representa una colección de experiencias laborales asociadas al currículum profesional.</returns>
    Task<IEnumerable<WorkExperience>> GetWorkExperiencesByProfessionalResumeId(Guid professionalResumeId);

    /// <summary>
    /// Reemplaza las experiencias laborales asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador único del currículum profesional.</param>
    /// <param name="workExperiences">Colección de experiencias laborales que reemplazarán las existentes.</param>
    /// <returns>Una tarea que representa un valor booleano indicando si la operación fue exitosa.</returns>
    Task<bool> ReplaceWorkExperiences(Guid professionalResumeId, IEnumerable<WorkExperience> workExperiences);
}