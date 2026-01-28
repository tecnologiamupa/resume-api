using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.RepositoryContracts;

/// <summary>
/// Define el contrato para el repositorio de currículums.
/// </summary>
public interface IResumeRepository
{
    /// <summary>
    /// Obtiene una lista de todos los currículums disponibles.
    /// </summary>
    /// <returns>Una tarea que representa una colección de objetos <see cref="ResumeInfo"/>.</returns>
    Task<IEnumerable<ResumeInfo?>> GetResumes();

    /// <summary>
    /// Obtiene una lista paginada de currículums.
    /// </summary>
    /// <param name="pageNumber">Número de página a recuperar.</param>
    /// <param name="pageSize">Cantidad de elementos por página.</param>
    /// <returns>Una tupla que contiene la colección de currículums y el total de registros.</returns>
    Task<(IEnumerable<ResumeInfo?> Resumes, int TotalRecords)> GetPagedResumes(int pageNumber, int pageSize);

    /// <summary>
    /// Obtiene una lista paginada de currículums que han sido completados.
    /// </summary>
    /// <param name="pageNumber">Número de página a recuperar.</param>
    /// <param name="pageSize">Cantidad de elementos por página.</param>
    /// <returns>Una tupla que contiene la colección de currículums completados y el total de registros.</returns>
    Task<(IEnumerable<ResumeInfo?> Resumes, int TotalRecords)> GetCompletedResumesPaged(int pageNumber, int pageSize);

    /// <summary>
    /// Obtiene una lista paginada de currículums que no han sido completados.
    /// </summary>
    /// <param name="pageNumber">Número de página a recuperar.</param>
    /// <param name="pageSize">Cantidad de elementos por página.</param>
    /// <returns>Una tupla que contiene la colección de currículums incompletos y el total de registros.</returns>
    Task<(IEnumerable<ResumeInfo?> Resumes, int TotalRecords)> GetIncompleteResumesPaged(int pageNumber, int pageSize);

    Task<(IEnumerable<ResumeInfo?> Resumes, int TotalRecords)> GetPagedResumesByFilter(ResumeFilterRequest filter, int pageNumber, int pageSize);

    Task<IEnumerable<ResumeWithVacancy>> GetResumesByCompany(Guid companyId);

    /// <summary>
    /// Obtiene un currículum específico por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del currículum.</param>
    /// <returns>Una tarea que representa el objeto <see cref="ResumeInfo"/> correspondiente, o null si no se encuentra.</returns>
    Task<ResumeInfo?> GetResumeById(Guid id);

    /// <summary>
    /// Obtiene un currículum específico por el identificador único de la información personal asociada.
    /// </summary>
    /// <param name="personalInfoId">El identificador único de la información personal.</param>
    /// <returns>Una tarea que representa el objeto <see cref="ResumeInfo"/> correspondiente, o null si no se encuentra.</returns>
    Task<ResumeInfo?> GetResumeByPersonalInfoId(Guid personalInfoId);

    /// <summary>
    /// Crea un nuevo currículum en el repositorio.
    /// </summary>
    /// <param name="resume">El objeto <see cref="ResumeInfo"/> que representa el currículum a crear.</param>
    /// <returns>Una tarea que representa el currículum creado.</returns>
    Task<ResumeInfo> CreateResume(ResumeInfo resume);

    /// <summary>
    /// Actualiza un currículum existente en el repositorio.
    /// </summary>
    /// <param name="resume">El objeto <see cref="ResumeInfo"/> que contiene los datos actualizados del currículum.</param>
    /// <returns>Una tarea que representa un valor booleano indicando si la actualización fue exitosa.</returns>
    Task<bool> UpdateResume(ResumeInfo resume);

    /// <summary>
    /// Elimina un currículum del repositorio por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del currículum a eliminar.</param>
    /// <returns>Una tarea que representa un valor booleano indicando si la eliminación fue exitosa.</returns>
    Task<bool> DeleteResume(Guid id);
}
