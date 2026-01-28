using Resume.Core.Entities;

namespace Resume.Core.RepositoryContracts;

/// <summary>
/// Define un contrato para las operaciones relacionadas con los currículums de prácticas.
/// </summary>
public interface IInternshipResumeRepository
{
    /// <summary>
    /// Obtiene un currículum de prácticas por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del currículum de prácticas.</param>
    /// <returns>El currículum de prácticas correspondiente o <c>null</c> si no se encuentra.</returns>
    Task<InternshipResume?> GetInternshipResumeById(Guid id);

    /// <summary>
    /// Obtiene un currículum de prácticas asociado a un currículum general por su identificador.
    /// </summary>
    /// <param name="resumeId">El identificador único del currículum general.</param>
    /// <returns>El currículum de prácticas correspondiente o <c>null</c> si no se encuentra.</returns>
    Task<InternshipResume?> GetInternshipResumeByResumeId(Guid resumeId);

    /// <summary>
    /// Crea un nuevo currículum de prácticas.
    /// </summary>
    /// <param name="internshipResume">El currículum de prácticas a crear.</param>
    /// <returns>El currículum de prácticas creado.</returns>
    Task<InternshipResume> CreateInternshipResume(InternshipResume internshipResume);

    /// <summary>
    /// Actualiza un currículum de prácticas existente.
    /// </summary>
    /// <param name="internshipResume">El currículum de prácticas con los datos actualizados.</param>
    /// <returns><c>true</c> si la actualización fue exitosa; de lo contrario, <c>false</c>.</returns>
    Task<bool> UpdateInternshipResume(InternshipResume internshipResume);

    /// <summary>
    /// Elimina un currículum de prácticas por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del currículum de prácticas a eliminar.</param>
    /// <returns><c>true</c> si la eliminación fue exitosa; de lo contrario, <c>false</c>.</returns>
    Task<bool> DeleteInternshipResume(Guid id);
}