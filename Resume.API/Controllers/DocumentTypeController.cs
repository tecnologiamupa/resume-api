using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resume.Core.ServiceContracts;

namespace Resume.API.Controllers
{
    [Authorize]
    [Route("api/document-types")]
    [ApiController]
    public class DocumentTypeController : ControllerBase
    {
        private readonly IDocumentTypeService _documentTypeService;

        /// <summary>
        /// Constructor de la clase <see cref="DocumentTypeController"/>.
        /// </summary>
        /// <param name="documentTypeService">Servicio para manejar la lógica de negocio de los tipos de documentos.</param>
        public DocumentTypeController(IDocumentTypeService documentTypeService)
        {
            _documentTypeService = documentTypeService;
        }

        /// <summary>
        /// Obtiene una lista de todos los tipos de documentos disponibles.
        /// </summary>
        /// <returns>
        /// Una respuesta HTTP que contiene una lista de tipos de documentos y un código de estado.
        /// </returns>
        [HttpGet] // GET api/document-types
        public async Task<IActionResult> GetDocumentTypes()
        {
            var documentTypesResponse = await _documentTypeService.GetDocumentTypes();
            return StatusCode(documentTypesResponse.StatusCode, documentTypesResponse);
        }
    }
}