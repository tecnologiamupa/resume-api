using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resume.Core.DTOs;
using Resume.Core.ServiceContracts;

namespace Resume.API.Controllers
{
    /// <summary>
    /// Controlador para gestionar operaciones sobre documentos de currículum.
    /// </summary>
    //[Authorize]
    [Route("api/resume-documents")]
    [ApiController]
    public class ResumeDocumentController : ControllerBase
    {
        private readonly IResumeDocumentService _resumeDocumentService;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ResumeDocumentController"/>.
        /// </summary>
        /// <param name="resumeDocumentService">Servicio de documentos de currículum.</param>
        public ResumeDocumentController(IResumeDocumentService resumeDocumentService)
        {
            _resumeDocumentService = resumeDocumentService;
        }

        /// <summary>
        /// Obtiene los documentos asociados a un currículum.
        /// </summary>
        /// <param name="resumeId">Identificador del currículum.</param>
        /// <returns>Lista de documentos del currículum.</returns>
        [HttpGet("resume/{resumeId:guid}")] // GET api/resume-documents/resume/{resumeId}
        public async Task<IActionResult> GetResumeDocumentsByResumeId(Guid resumeId)
        {
            var documentsResponse = await _resumeDocumentService.GetResumeDocumentsByResumeId(resumeId);
            return StatusCode(documentsResponse.StatusCode, documentsResponse);
        }

        /// <summary>
        /// Sube un nuevo documento para un currículum.
        /// </summary>
        /// <param name="request">Datos del documento a subir.</param>
        /// <returns>Resultado de la creación del documento.</returns>
        [Authorize]
        [HttpPost] // POST api/resume-documents
        public async Task<IActionResult> CreateResumeDocument([FromForm] ResumeDocumentCreateRequest request)
        {
            var createResponse = await _resumeDocumentService.CreateResumeDocument(request);
            return StatusCode(createResponse.StatusCode, createResponse);
        }

        /// <summary>
        /// Elimina un documento de currículum por su identificador.
        /// </summary>
        /// <param name="id">Identificador del documento.</param>
        /// <returns>Resultado de la eliminación.</returns>
        [Authorize]
        [HttpDelete("{id:guid}")] // DELETE api/resume-documents/{id}
        public async Task<IActionResult> DeleteResumeDocument(Guid id)
        {
            var deleteResponse = await _resumeDocumentService.DeleteResumeDocument(id);
            return StatusCode(deleteResponse.StatusCode, deleteResponse);
        }
    }
}