using Microsoft.AspNetCore.Mvc;
using Resume.Core.DTOs;
using Resume.Core.ServiceContracts;

namespace Resume.API.Controllers
{
    /// <summary>
    /// Controlador para gestionar la relación entre eventos y currículums.
    /// </summary>
    [Route("api/event-resume")]
    [ApiController]
    public class EventResumeController : ControllerBase
    {
        private readonly IEventResumeService _eventResumeService;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="EventResumeController"/>.
        /// </summary>
        /// <param name="eventResumeService">Servicio para la relación evento-currículum.</param>
        public EventResumeController(IEventResumeService eventResumeService)
        {
            _eventResumeService = eventResumeService;
        }

        /// <summary>
        /// Obtiene la relación evento-currículum por identificador de currículum y evento.
        /// </summary>
        /// <param name="resumeId">Identificador del currículum.</param>
        /// <param name="eventId">Identificador del evento.</param>
        /// <returns>Detalle de la relación evento-currículum.</returns>
        [HttpGet("{resumeId:guid}/{eventId:int}")] // GET api/event-resume/{resumeId}/{eventId}
        public async Task<IActionResult> GetEventResumeByResumeId(Guid resumeId, int eventId)
        {
            var response = await _eventResumeService.GetEventResumeByResumeId(resumeId, eventId);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Obtiene una lista paginada de relaciones evento-currículum filtradas.
        /// </summary>
        /// <param name="filter">Criterios de filtro para las relaciones evento-currículum.</param>
        /// <param name="pageNumber">Número de página a recuperar.</param>
        /// <param name="pageSize">Cantidad de elementos por página.</param>
        /// <returns>Lista paginada de relaciones evento-currículum filtradas.</returns>
        [HttpPost("filter")] // POST api/event-resumes/filter
        public async Task<IActionResult> GetPagedEventResumesByFilter(
            [FromBody] EventResumeFilterRequest filter,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (filter == null)
            {
                return BadRequest(BaseResponse<string>.Fail("Datos de solicitud inválidos."));
            }

            var response = await _eventResumeService.GetPagedEventResumesByFilter(filter, pageNumber, pageSize);

            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Crea una nueva relación evento-currículum.
        /// </summary>
        /// <param name="eventResumeRequest">Datos para crear la relación evento-currículum.</param>
        /// <returns>Resultado de la creación.</returns>
        [HttpPost] // POST api/event-resume
        public async Task<IActionResult> CreateEventResume([FromBody] EventResumeCreateRequest eventResumeRequest)
        {
            if (eventResumeRequest == null)
            {
                return BadRequest("Datos de solicitud inválidos.");
            }

            var response = await _eventResumeService.CreateEventResume(eventResumeRequest);
            return StatusCode(response.StatusCode, response);
        }
    }
}