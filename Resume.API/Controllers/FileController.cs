using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resume.Core.ServiceContracts;

namespace Resume.API.Controllers
{
    //[Authorize]
    [Route("api/files")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly ISftpFileService _sftpFileService;

        public FileController(ISftpFileService sftpFileService)
        {
            _sftpFileService = sftpFileService;
        }

        [HttpGet("{*relativePath}")]
        public async Task<IActionResult> DownloadFile(string relativePath)
        {
            try
            {
                byte[] fileData = await _sftpFileService.DownloadFile(relativePath);
                string contentType = GetContentType(relativePath); // Método auxiliar para determinar el tipo MIME
                return File(fileData, contentType, relativePath);
            }
            catch (FileNotFoundException)
            {
                return NotFound("Archivo no encontrado.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al descargar el archivo: {ex.Message}");
            }
        }

        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".txt" => "text/plain",
                ".jpg" => "image/jpeg",
                ".png" => "image/png",
                ".pdf" => "application/pdf",
                _ => "application/octet-stream",
            };
        }
    }
}
