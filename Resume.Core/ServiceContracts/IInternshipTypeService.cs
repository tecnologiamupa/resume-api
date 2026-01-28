using Resume.Core.DTOs;

namespace Resume.Core.ServiceContracts;

/// <summary>
/// Define los contratos de servicio relacionados con los tipos de pasantías.
/// </summary>
public interface IInternshipTypeService
{
    /// <summary>
    /// Obtiene una lista de tipos de pasantías.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El resultado contiene una respuesta base con una lista de objetos <see cref="InternshipTypeResponse"/>.
    /// </returns>
    Task<BaseResponse<List<InternshipTypeResponse?>>> GetInternshipTypes();

    /// <summary>
    /// Obtiene un tipo de pasantía específico por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del tipo de pasantía.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El resultado contiene una respuesta base con un objeto <see cref="InternshipTypeResponse"/> correspondiente al identificador proporcionado.
    /// </returns>
    Task<BaseResponse<InternshipTypeResponse?>> GetInternshipTypeById(int id);
}
