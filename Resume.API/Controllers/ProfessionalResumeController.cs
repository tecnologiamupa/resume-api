using Microsoft.AspNetCore.Mvc;
using Resume.Core.DTOs;
using Resume.Core.ServiceContracts;

namespace Resume.API.Controllers
{
    [Route("api/professional-resume")]
    [ApiController]
    public class ProfessionalResumeController : ControllerBase
    {
        private readonly IProfessionalResumeService _professionalResumeService;

        public ProfessionalResumeController(IProfessionalResumeService professionalResumeService)
        {
            _professionalResumeService = professionalResumeService;
        }

        /// <summary>
        /// Actualiza en lote los campos Platzi de currículums profesionales.
        /// </summary>
        /// <param name="requests">Lista de datos para actualizar los campos Platzi.</param>
        /// <returns>Resultado de la actualización masiva.</returns>
        [HttpPut("platzi/batch")]
        public async Task<IActionResult> UpdatePlatziFieldsBatch([FromBody] IEnumerable<ProfessionalResumePlatziUpdateRequest> requests)
        {
            if (requests == null)
                return BadRequest("La solicitud no puede ser nula.");

            var response = await _professionalResumeService.UpdatePlatziFieldsByResumeListAsync(requests);
            return StatusCode(response.StatusCode, response);
        }
    }
}