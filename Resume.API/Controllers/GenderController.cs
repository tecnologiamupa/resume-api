using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resume.Core.ServiceContracts;

namespace Resume.API.Controllers
{
    [Authorize]
    [Route("api/genders")]
    [ApiController]
    public class GenderController : ControllerBase
    {
        private readonly IGenderService _genderService;

        /// <summary>
        /// Constructor de la clase <see cref="GenderController"/>.
        /// </summary>
        /// <param name="genderService">Servicio para manejar la lógica de negocio de los géneros.</param>
        public GenderController(IGenderService genderService)
        {
            _genderService = genderService;
        }

        /// <summary>
        /// Obtiene una lista de todos los géneros disponibles.
        /// </summary>
        /// <returns>
        /// Una respuesta HTTP que contiene una lista de géneros y un código de estado.
        /// </returns>
        [HttpGet] // GET api/genders
        public async Task<IActionResult> GetGenders()
        {
            var gendersResponse = await _genderService.GetGenders();
            return StatusCode(gendersResponse.StatusCode, gendersResponse);
        }
    }
}