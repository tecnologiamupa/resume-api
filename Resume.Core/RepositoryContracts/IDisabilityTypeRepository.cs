using Resume.Core.Entities;

namespace Resume.Core.RepositoryContracts;

/// <summary>
/// Define los métodos para interactuar con los datos relacionados con los tipos de discapacidad.
/// </summary>
public interface IDisabilityTypeRepository
{
    /// <summary>
    /// Obtiene una lista de todos los tipos de discapacidad disponibles.
    /// </summary>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado contiene una colección de objetos <see cref="DisabilityType"/>.</returns>
    Task<IEnumerable<DisabilityType?>> GetDisabilityTypes();

    /// <summary>
    /// Obtiene un tipo de discapacidad específico por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del tipo de discapacidad.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado contiene el objeto <see cref="DisabilityType"/> correspondiente o <c>null</c> si no se encuentra.</returns>
    Task<DisabilityType?> GetDisabilityTypeById(int id);
}
