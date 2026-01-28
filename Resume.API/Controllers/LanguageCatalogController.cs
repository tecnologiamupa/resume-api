using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resume.Core.ServiceContracts;

namespace Resume.API.Controllers
{
    //[Authorize]
    [Route("api/languages-catalog")]
    [ApiController]
    public class LanguageCatalogController : ControllerBase
    {
        private readonly ILanguageCatalogService _languageCatalogService;

        /// <summary>
        /// Constructor de la clase <see cref="LanguageCatalogController"/>.
        /// </summary>
        /// <param name="languageCatalogService">Servicio para manejar la lógica de negocio del catálogo de idiomas.</param>
        public LanguageCatalogController(ILanguageCatalogService languageCatalogService)
        {
            _languageCatalogService = languageCatalogService;
        }

        /// <summary>
        /// Obtiene una lista de todos los idiomas disponibles en el catálogo.
        /// </summary>
        /// <returns>
        /// Una respuesta HTTP que contiene una lista de idiomas y un código de estado.
        /// </returns>
        [HttpGet] // GET api/languages-catalog
        public async Task<IActionResult> GetLanguagesCatalog()
        {
            var languagesResponse = await _languageCatalogService.GetLanguagesCatalog();
            return StatusCode(languagesResponse.StatusCode, languagesResponse);
        }

        /// <summary>
        /// Obtiene un idioma específico del catálogo por su identificador.
        /// </summary>
        /// <param name="id">Identificador del idioma.</param>
        /// <returns>
        /// Una respuesta HTTP que contiene el idioma solicitado y un código de estado.
        /// </returns>
        [HttpGet("{id}")] // GET api/languages-catalog/{id}
        public async Task<IActionResult> GetLanguageCatalogById(int id)
        {
            var languageResponse = await _languageCatalogService.GetLanguageCatalogById(id);
            return StatusCode(languageResponse.StatusCode, languageResponse);
        }
    }
}