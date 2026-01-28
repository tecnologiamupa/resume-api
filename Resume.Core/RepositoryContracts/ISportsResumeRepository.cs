using Resume.Core.Entities;

namespace Resume.Core.RepositoryContracts;

/// <summary>
/// Define un contrato para las operaciones relacionadas con los currículums deportivos.
/// </summary>
public interface ISportsResumeRepository
{
    /// <summary>
    /// Obtiene un currículum deportivo por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del currículum deportivo.</param>
    /// <returns>El currículum deportivo correspondiente o <c>null</c> si no se encuentra.</returns>
    Task<SportsResume?> GetSportsResumeById(Guid id);

    /// <summary>
    /// Obtiene un currículum deportivo asociado a un currículum general por el identificador del currículum general.
    /// </summary>
    /// <param name="resumeId">El identificador único del currículum general.</param>
    /// <returns>El currículum deportivo correspondiente o <c>null</c> si no se encuentra.</returns>
    Task<SportsResume?> GetSportsResumeByResumeId(Guid resumeId);

    /// <summary>
    /// Crea un nuevo currículum deportivo.
    /// </summary>
    /// <param name="sportsResume">El currículum deportivo a crear.</param>
    /// <returns>El currículum deportivo creado.</returns>
    Task<SportsResume> CreateSportsResume(SportsResume sportsResume);

    /// <summary>
    /// Actualiza un currículum deportivo existente.
    /// </summary>
    /// <param name="sportsResume">El currículum deportivo con los datos actualizados.</param>
    /// <returns><c>true</c> si la actualización fue exitosa; de lo contrario, <c>false</c>.</returns>
    Task<bool> UpdateSportsResume(SportsResume sportsResume);

    /// <summary>
    /// Elimina un currículum deportivo por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del currículum deportivo a eliminar.</param>
    /// <returns><c>true</c> si la eliminación fue exitosa; de lo contrario, <c>false</c>.</returns>
    Task<bool> DeleteSportsResume(Guid id);
}