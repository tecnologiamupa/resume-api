using Resume.Core.DTOs;

namespace Resume.Core.ServiceContracts;

/// <summary>
/// Define el contrato para los servicios relacionados con los cursos de INADEH.
/// </summary>
public interface IInadehCourseService
{
    /// <summary>
    /// Obtiene la lista de cursos de INADEH.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. 
    /// El resultado contiene una respuesta base con una lista de objetos <see cref="InadehCourseResponse"/>.
    /// </returns>
    Task<BaseResponse<List<InadehCourseResponse?>>> GetInadehCourses();

    /// <summary>
    /// Obtiene un curso de INADEH por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del curso.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. 
    /// El resultado contiene una respuesta base con un objeto <see cref="InadehCourseResponse"/>.
    /// </returns>
    Task<BaseResponse<InadehCourseResponse?>> GetInadehCourseById(int id);
}
