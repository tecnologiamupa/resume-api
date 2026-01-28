using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resume.Core.DTOs;
using Resume.Core.Entities;
using Resume.Core.Helpers;
using Resume.Core.RepositoryContracts;
using Resume.Core.ServiceContracts;
using Sigueme.Core.DTOs;
using Resume.Core.DTOs.ResumeInfo;

namespace Resume.API.Controllers
{
    /// <summary>
    /// Controlador para gestionar operaciones sobre currículums.
    /// </summary>    
    [Route("api/resumes")]
    [ApiController]
    public class ResumeController : ControllerBase
    {
        private readonly IResumeService _resumeService;
        private readonly IApiLogRepository _apiLogRepository;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ResumeController"/>.
        /// </summary>
        /// <param name="resumeService">Servicio de currículums.</param>
        public ResumeController(
            IResumeService resumeService,
            IApiLogRepository apiLogRepository)
        {
            _resumeService = resumeService;
            _apiLogRepository = apiLogRepository;
        }

        /// <summary>
        /// Obtiene una lista paginada de todos los currículums.
        /// </summary>
        /// <param name="pageNumber">Número de página a recuperar.</param>
        /// <param name="pageSize">Cantidad de elementos por página.</param>
        /// <returns>Lista paginada de currículums.</returns>
        [HttpGet] // GET api/resumes
        public async Task<IActionResult> GetPagedResumes([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            // Llama al servicio para obtener los currículums paginados
            var pagedResumesResponse = await _resumeService.GetPagedResumes(pageNumber, pageSize);

            return StatusCode(pagedResumesResponse.StatusCode, pagedResumesResponse);
        }

        /// <summary>
        /// Obtiene una lista paginada de currículums filtrados.
        /// </summary>
        /// <param name="filter">Criterios de filtro para los currículums.</param>
        /// <param name="pageNumber">Número de página a recuperar.</param>
        /// <param name="pageSize">Cantidad de elementos por página.</param>
        /// <returns>Lista paginada de currículums filtrados.</returns>
        [HttpPost("filter")] // POST api/resumes/filter
        public async Task<IActionResult> GetPagedResumesByFilter([FromBody] ResumeFilterRequest filter, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (filter == null)
            {
                return BadRequest(BaseResponse<string>.Fail("Datos de solicitud inválidos."));
            }

            var resumesResponse = await _resumeService.GetPagedResumesByFilter(filter, pageNumber, pageSize);

            return StatusCode(resumesResponse.StatusCode, resumesResponse);
        }

        /// <summary>
        /// Obtiene una lista paginada de currículums completados.
        /// </summary>
        /// <param name="pageNumber">Número de página a recuperar.</param>
        /// <param name="pageSize">Cantidad de elementos por página.</param>
        /// <returns>Lista paginada de currículums completados.</returns>
        [HttpGet("completed")] // GET api/resumes/completed
        public async Task<IActionResult> GetCompletedResumesPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            // Llama al servicio para obtener los currículums completados paginados
            var completedResumesResponse = await _resumeService.GetCompletedResumesPaged(pageNumber, pageSize);

            return StatusCode(completedResumesResponse.StatusCode, completedResumesResponse);
        }

        /// <summary>
        /// Obtiene una lista paginada de currículums incompletos.
        /// </summary>
        /// <param name="pageNumber">Número de página a recuperar.</param>
        /// <param name="pageSize">Cantidad de elementos por página.</param>
        /// <returns>Lista paginada de currículums incompletos.</returns>
        [HttpGet("incomplete")] // GET api/resumes/incomplete
        public async Task<IActionResult> GetIncompleteResumesPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            // Llama al servicio para obtener los currículums incompletos paginados
            var incompleteResumesResponse = await _resumeService.GetIncompleteResumesPaged(pageNumber, pageSize);

            return StatusCode(incompleteResumesResponse.StatusCode, incompleteResumesResponse);
        }

        /// <summary>
        /// Obtiene la lista de currículums favoritos de una empresa, permitiendo duplicados por vacante y mostrando el nombre de la vacante.
        /// </summary>
        /// <param name="companyId">Identificador de la empresa.</param>
        /// <returns>Lista de currículums con el nombre de la vacante.</returns>
        [HttpGet("company/{companyId:guid}")] // GET api/resumes/company/{companyId}
        public async Task<IActionResult> GetResumesByCompany(Guid companyId)
        {
            var resumesResponse = await _resumeService.GetResumesByCompany(companyId);
            return StatusCode(resumesResponse.StatusCode, resumesResponse);
        }

        /// <summary>
        /// Obtiene el currículum del usuario autenticado.
        /// </summary>
        /// <returns>Detalle del currículum del usuario autenticado.</returns>
        [Authorize]
        [HttpGet("me")] // GET api/resumes/me
        public async Task<IActionResult> GetMyResume()
        {
            var resumeResponse = await _resumeService.GetMyResume();
            return StatusCode(resumeResponse.StatusCode, resumeResponse);
        }

        /// <summary>
        /// Obtiene un currículum por su identificador.
        /// </summary>
        /// <param name="id">Identificador del currículum.</param>
        /// <returns>Detalle del currículum.</returns>
        //[Authorize]
        [HttpGet("{id:guid}")] // GET api/resumes/{id}
        public async Task<IActionResult> GetResumeById(Guid id)
        {
            var resumeResponse = await _resumeService.GetResumeById(id);
            return StatusCode(resumeResponse.StatusCode, resumeResponse);
        }

        /// <summary>
        /// Crea un nuevo currículum.
        /// </summary>
        /// <param name="resume">Datos para crear el currículum.</param>
        /// <returns>Resultado de la creación.</returns>
        [HttpPost] // POST api/resumes
        public async Task<IActionResult> CreateResume([FromBody] ResumeCreateRequest resume)
        {
            var logEntry = ApiLogHelper.MapFromRequest(HttpContext, resume, "CreateResume llamado.");

            await _apiLogRepository.InsertApiLog(logEntry);

            var createResponse = await _resumeService.CreateResume(resume);
            return StatusCode(createResponse.StatusCode, createResponse);
        }

        /// <summary>
        /// Actualiza un currículum existente.
        /// </summary>
        /// <param name="id">Identificador del currículum.</param>
        /// <param name="resume">Datos para actualizar el currículum.</param>
        /// <returns>Resultado de la actualización.</returns>
        [HttpPut("{id:guid}")] // PUT api/resumes/{id}
        public async Task<IActionResult> UpdateResume(Guid id, [FromBody] ResumeUpdateRequest resume)
        {
            var logEntry = ApiLogHelper.MapFromRequest(HttpContext, resume, $"UpdateResume llamado. ResumeId: {id}");

            await _apiLogRepository.InsertApiLog(logEntry);

            var updateResponse = await _resumeService.UpdateResume(id, resume);
            return StatusCode(updateResponse.StatusCode, updateResponse);
        }

        /// <summary>
        /// Elimina un currículum por su identificador.
        /// </summary>
        /// <param name="id">Identificador del currículum.</param>
        /// <returns>Resultado de la eliminación.</returns>
        //[Authorize]
        //[HttpDelete("{id:guid}")] // DELETE api/resumes/{id}
        //public async Task<IActionResult> DeleteResume(Guid id)
        //{
        //    var deleteResponse = await _resumeService.DeleteResume(id);
        //    return StatusCode(deleteResponse.StatusCode, deleteResponse);
        //}

        /// <summary>
        /// Obtiene el detalle de un currículum por datos de login (identityNumber, mobile, etc).
        /// </summary>
        /// <param name="request">Datos de login para buscar el currículum.</param>
        /// <returns>Detalle del currículum.</returns>
        [HttpPost("login-custom")]
        public async Task<IActionResult> GetResumeByLogin([FromBody] ResumeLoginRequest request)
        {
            if (request == null)
                return BadRequest(BaseResponse<string>.Fail("Datos de solicitud inválidos."));

            var response = await _resumeService.GetResumeByLoginRequest(request);
            return StatusCode(response.StatusCode, response);
        }
    }
}
