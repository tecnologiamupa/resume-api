using Resume.Core.Entities;

namespace Resume.Core.RepositoryContracts;

/// <summary>
/// Define un contrato para interactuar con los datos de los cursos de INADEH.
/// </summary>
public interface IInadehCourseRepository
{
    /// <summary>
    /// Obtiene una lista de todos los cursos de INADEH disponibles.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El valor de retorno contiene una colección de objetos <see cref="InadehCourse"/>.
    /// </returns>
    Task<IEnumerable<InadehCourse?>> GetInadehCourses();

    /// <summary>
    /// Obtiene un curso de INADEH específico por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del curso.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El valor de retorno contiene un objeto <see cref="InadehCourse"/> si se encuentra; de lo contrario, <c>null</c>.
    /// </returns>
    Task<InadehCourse?> GetInadehCourseById(int id);
}
