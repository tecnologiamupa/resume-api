using Microsoft.AspNetCore.Http;
using Resume.Core.DTOs;

namespace Resume.Core.ServiceContracts;

/// <summary>
/// Interfaz para el servicio de gestión de imágenes de perfil.
/// </summary>
public interface IProfileImageService
{
    /// <summary>
    /// Sube una imagen de perfil asociada a la información personal de un usuario.
    /// </summary>
    /// <param name="request">Objeto que contiene los datos necesarios para guardar la imagen de perfil.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El resultado contiene una respuesta base con los datos de la imagen de perfil subida.
    /// </returns>
    Task<BaseResponse<ProfileImageResponse?>> UploadProfileImage(ProfileImageSaveRequest request);
}
