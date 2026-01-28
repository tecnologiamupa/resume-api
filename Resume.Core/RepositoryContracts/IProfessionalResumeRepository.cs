using Resume.Core.Entities;

namespace Resume.Core.RepositoryContracts;

/// <summary>
/// Define un contrato para las operaciones relacionadas con la entidad <see cref="ProfessionalResume"/>.
/// Proporciona métodos para obtener, crear, actualizar y eliminar currículums profesionales.
/// </summary>
public interface IProfessionalResumeRepository
{
    /// <summary>
    /// Obtiene un currículum profesional por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del currículum profesional.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado contiene el currículum profesional si se encuentra; de lo contrario, <c>null</c>.</returns>
    Task<ProfessionalResume?> GetProfessionalResumeById(Guid id);

    /// <summary>
    /// Obtiene un currículum profesional asociado a un currículum específico por su identificador.
    /// </summary>
    /// <param name="resumeId">El identificador único del currículum asociado.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado contiene el currículum profesional si se encuentra; de lo contrario, <c>null</c>.</returns>
    Task<ProfessionalResume?> GetProfessionalResumeByResumeId(Guid resumeId);

    /// <summary>
    /// Crea un nuevo currículum profesional.
    /// </summary>
    /// <param name="professionalResume">La entidad <see cref="ProfessionalResume"/> que se va a crear.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado contiene la entidad creada.</returns>
    Task<ProfessionalResume> CreateProfessionalResume(ProfessionalResume professionalResume);

    /// <summary>
    /// Actualiza un currículum profesional existente.
    /// </summary>
    /// <param name="professionalResume">La entidad <see cref="ProfessionalResume"/> con los datos actualizados.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado es <c>true</c> si la actualización fue exitosa; de lo contrario, <c>false</c>.</returns>
    Task<bool> UpdateProfessionalResume(ProfessionalResume professionalResume);

    Task<bool> UpdatePlatziFieldsByResumeListAsync(IEnumerable<ProfessionalResume> resumes);

    /// <summary>
    /// Elimina un currículum profesional por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del currículum profesional a eliminar.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado es <c>true</c> si la eliminación fue exitosa; de lo contrario, <c>false</c>.</returns>
    Task<bool> DeleteProfessionalResume(Guid id);
}
