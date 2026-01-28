using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resume.Core.ServiceContracts;

namespace Resume.API.Controllers
{
    //[Authorize]
    [Route("api/skills-catalog")]
    [ApiController]
    public class SkillCatalogController : ControllerBase
    {
        private readonly ISkillCatalogService _skillCatalogService;

        /// <summary>
        /// Constructor de la clase <see cref="SkillCatalogController"/>.
        /// </summary>
        /// <param name="skillCatalogService">Servicio para manejar la lógica de negocio del catálogo de habilidades.</param>
        public SkillCatalogController(ISkillCatalogService skillCatalogService)
        {
            _skillCatalogService = skillCatalogService;
        }

        /// <summary>
        /// Obtiene una lista de todas las habilidades disponibles en el catálogo.
        /// </summary>
        /// <returns>
        /// Una respuesta HTTP que contiene una lista de habilidades y un código de estado.
        /// </returns>
        [HttpGet] // GET api/skills-catalog
        public async Task<IActionResult> GetSkillsCatalog()
        {
            var skillsResponse = await _skillCatalogService.GetSkillsCatalog();
            return StatusCode(skillsResponse.StatusCode, skillsResponse);
        }

        /// <summary>
        /// Obtiene una habilidad específica del catálogo por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la habilidad.</param>
        /// <returns>
        /// Una respuesta HTTP que contiene la habilidad solicitada y un código de estado.
        /// </returns>
        [HttpGet("{id}")] // GET api/skills-catalog/{id}
        public async Task<IActionResult> GetSkillCatalogById(int id)
        {
            var skillResponse = await _skillCatalogService.GetSkillCatalogById(id);
            return StatusCode(skillResponse.StatusCode, skillResponse);
        }
    }
}