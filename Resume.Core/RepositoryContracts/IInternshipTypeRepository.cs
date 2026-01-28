using Resume.Core.Entities;

namespace Resume.Core.RepositoryContracts;

/// <summary>
/// Define un contrato para interactuar con los tipos de pasantías en el sistema.
/// </summary>
public interface IInternshipTypeRepository
{
    /// <summary>
    /// Obtiene una colección de tipos de pasantías disponibles.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. 
    /// El resultado contiene una colección de objetos <see cref="InternshipType"/>.
    /// </returns>
    Task<IEnumerable<InternshipType?>> GetInternshipTypes();

    /// <summary>
    /// Obtiene un tipo de pasantía específico basado en su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del tipo de pasantía.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. 
    /// El resultado contiene un objeto <see cref="InternshipType"/> si se encuentra; de lo contrario, <c>null</c>.
    /// </returns>
    Task<InternshipType?> GetInternshipTypeById(int id);
}
