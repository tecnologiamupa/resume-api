using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resume.Core.ServiceContracts;

namespace Resume.API.Controllers
{
    [Authorize]
    [Route("api/inadeh-courses")]
    [ApiController]
    public class InadehCourseController : ControllerBase
    {
        private readonly IInadehCourseService _inadehCourseService;
        /// <summary>
        /// Constructor de la clase <see cref="InadehCourseController"/>.
        /// </summary>
        /// <param name="inadehCourseService">Servicio para manejar la lógica de negocio de los cursos de INADEH.</param>
        public InadehCourseController(IInadehCourseService inadehCourseService)
        {
            _inadehCourseService = inadehCourseService;
        }
        /// <summary>
        /// Obtiene una lista de todos los cursos de INADEH disponibles.
        /// </summary>
        /// <returns>
        /// Una respuesta HTTP que contiene una lista de cursos de INADEH y un código de estado.
        /// </returns>
        [HttpGet] // GET api/inadeh-courses
        public async Task<IActionResult> GetInadehCourses()
        {
            var inadehCoursesResponse = await _inadehCourseService.GetInadehCourses();
            return StatusCode(inadehCoursesResponse.StatusCode, inadehCoursesResponse);
        }
    }
}
