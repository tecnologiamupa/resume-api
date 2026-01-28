using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resume.Core.ServiceContracts;

namespace Resume.API.Controllers
{
    [Authorize]
    [Route("api/disability-types")]
    [ApiController]
    public class DisabilityTypeController : ControllerBase
    {
        private readonly IDisabilityTypeService _disabilityTypeService;

        /// <summary>
        /// Constructor de la clase <see cref="DisabilityTypeController"/>.
        /// </summary>
        /// <param name="disabilityTypeService">Servicio para manejar la lógica de negocio de los tipos de discapacidad.</param>
        public DisabilityTypeController(IDisabilityTypeService disabilityTypeService)
        {
            _disabilityTypeService = disabilityTypeService;
        }

        /// <summary>
        /// Obtiene una lista de todos los tipos de discapacidad disponibles.
        /// </summary>
        /// <returns>
        /// Una respuesta HTTP que contiene una lista de tipos de discapacidad y un código de estado.
        /// </returns>
        [HttpGet] // GET api/disability-types
        public async Task<IActionResult> GetDisabilityTypes()
        {
            var disabilityTypesResponse = await _disabilityTypeService.GetDisabilityTypes();
            return StatusCode(disabilityTypesResponse.StatusCode, disabilityTypesResponse);
        }
    }
}