using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.RepositoryContracts;
using Resume.Core.ServiceContracts;

namespace Resume.Core.Services;

/// <summary>
/// Servicio para manejar la lógica de negocio relacionada con los cursos de INADEH.
/// </summary>
internal class InadehCourseService : IInadehCourseService
{
    private readonly IInadehCourseRepository _inadehCourseRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor de la clase <see cref="InadehCourseService"/>.
    /// </summary>
    /// <param name="inadehCourseRepository">Repositorio para interactuar con los datos de los cursos de INADEH.</param>
    /// <param name="mapper">Instancia de <see cref="IMapper"/> para mapear entre entidades y DTOs.</param>
    public InadehCourseService(IInadehCourseRepository inadehCourseRepository, IMapper mapper)
    {
        _inadehCourseRepository = inadehCourseRepository;
        _mapper = mapper;
    }    

    /// <summary>
    /// Obtiene la lista de cursos de INADEH.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. 
    /// El resultado contiene una respuesta base con una lista de objetos <see cref="InadehCourseResponse"/>.
    /// </returns>
    public async Task<BaseResponse<List<InadehCourseResponse?>>> GetInadehCourses()
    {
        // Obtiene los cursos desde el repositorio.
        var inadehCourses = await _inadehCourseRepository.GetInadehCourses();

        // Mapea los cursos a objetos de respuesta.
        var responses = _mapper.Map<List<InadehCourseResponse?>>(inadehCourses);

        // Retorna una respuesta exitosa con los datos mapeados.
        return BaseResponse<List<InadehCourseResponse?>>.Success(responses);
    }

    /// <summary>
    /// Obtiene un curso de INADEH por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del curso.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. 
    /// El resultado contiene una respuesta base con un objeto <see cref="InadehCourseResponse"/>.
    /// </returns>
    public async Task<BaseResponse<InadehCourseResponse?>> GetInadehCourseById(int id)
    {
        // Obtiene el curso desde el repositorio por su ID.
        var inadehCourse = await _inadehCourseRepository.GetInadehCourseById(id);

        // Verifica si el curso no fue encontrado.
        if (inadehCourse == null)
        {
            return BaseResponse<InadehCourseResponse?>.Fail("Curso de INADEH no encontrado.");
        }

        var response = _mapper.Map<InadehCourseResponse?>(inadehCourse);

        // Retorna una respuesta exitosa con los datos mapeados.
        return BaseResponse<InadehCourseResponse?>.Success(response);
    }
}
