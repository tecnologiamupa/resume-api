using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resume.Core.ServiceContracts;

namespace Resume.API.Controllers
{
    /// <summary>
    /// Controlador para gestionar las operaciones relacionadas con los tipos de pasantías.
    /// </summary>
    [Authorize]
    [Route("api/internship-types")]
    [ApiController]
    public class InternshipTypeController : ControllerBase
    {
        private readonly IInternshipTypeService _internshipTypeService;

        /// <summary>
        /// Constructor de la clase <see cref="InternshipTypeController"/>.
        /// </summary>
        /// <param name="internshipTypeService">Servicio para manejar la lógica de negocio de los tipos de pasantías.</param>
        public InternshipTypeController(IInternshipTypeService internshipTypeService)
        {
            _internshipTypeService = internshipTypeService;
        }

        /// <summary>
        /// Obtiene una lista de todos los tipos de pasantías disponibles.
        /// </summary>
        /// <returns>
        /// Una respuesta HTTP que contiene una lista de tipos de pasantías y un código de estado.
        /// </returns>
        /// <response code="200">Si la operación se realizó exitosamente.</response>
        /// <response code="400">Si ocurrió un error en la solicitud.</response>
        [HttpGet] // GET api/internship-type
        public async Task<IActionResult> GetInternshipTypes()
        {
            var internshipTypesResponse = await _internshipTypeService.GetInternshipTypes();
            return StatusCode(internshipTypesResponse.StatusCode, internshipTypesResponse);
        }
    }
}
