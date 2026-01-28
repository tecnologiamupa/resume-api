using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resume.Core.DTOs;
using Resume.Core.ServiceContracts;

namespace Resume.API.Controllers
{
    [Authorize]
    [Route("api/profile-images")]
    [ApiController]
    public class ProfileImageController : ControllerBase
    {
        private readonly IProfileImageService _profileImageService;

        /// <summary>
        /// Constructor de la clase <see cref="ProfileImageController"/>.
        /// </summary>
        /// <param name="profileImageService">Servicio para la gestión de imágenes de perfil.</param>
        public ProfileImageController(IProfileImageService profileImageService)
        {
            _profileImageService = profileImageService;
        }

        /// <summary>
        /// Sube una imagen de perfil para un usuario.
        /// </summary>
        /// <param name="request">Objeto que contiene los datos necesarios para subir la imagen de perfil.</param>
        /// <returns>
        /// Una respuesta HTTP que contiene la URL de la imagen subida si la operación es exitosa,
        /// o un mensaje de error en caso contrario.
        /// </returns>
        /// <response code="200">La imagen de perfil fue subida exitosamente.</response>
        /// <response code="400">La solicitud contiene datos inválidos.</response>
        /// <response code="500">Ocurrió un error interno en el servidor.</response>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadProfileImage([FromForm] ProfileImageSaveRequest request)
        {
            var response = await _profileImageService.UploadProfileImage(request);

            return StatusCode(response.StatusCode, response);
        }
    }
}
