using Resume.Core.Entities;

namespace Resume.Core.RepositoryContracts;

/// <summary>
/// Define un contrato para la gestión de entidades de tipo <see cref="Gender"/>.
/// </summary>
public interface IGenderRepository
{
    /// <summary>
    /// Obtiene una lista de todos los géneros disponibles.
    /// </summary>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado contiene una colección de objetos <see cref="Gender"/> o nulos.</returns>
    Task<IEnumerable<Gender?>> GetGenders();

    /// <summary>
    /// Obtiene un género específico por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del género.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado contiene un objeto <see cref="Gender"/> o nulo si no se encuentra.</returns>
    Task<Gender?> GetGenderById(int id);
}
