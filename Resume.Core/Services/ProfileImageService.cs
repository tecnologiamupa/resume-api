using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Resume.Core.DTOs;
using Resume.Core.Entities;
using Resume.Core.Helpers;
using Resume.Core.RepositoryContracts;
using Resume.Core.ServiceContracts;

namespace Resume.Core.Services;

internal class ProfileImageService : IProfileImageService
{
    private readonly IPersonalInfoRepository _personalInfoRepository;
    private readonly ISftpFileService _sftpFileService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _imageProfileFolder;
    private readonly long _maxProfileImageSize;

    public ProfileImageService(
        IPersonalInfoRepository personalInfoRepository,
        IConfiguration configuration,
        ISftpFileService sftpFileService,
        IHttpContextAccessor httpContextAccessor)
    {
        _personalInfoRepository = personalInfoRepository;
        _imageProfileFolder = configuration["Files:ProfileFolder"] ?? string.Empty;
        _maxProfileImageSize = long.Parse(configuration["Files:MaxProfileImageSize"] ?? "5242880"); // 5 MB por defecto
        _sftpFileService = sftpFileService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<BaseResponse<ProfileImageResponse?>> UploadProfileImage(ProfileImageSaveRequest request)
    {
        // Verificar si el PersonalInfo existe
        var personalInfo = await _personalInfoRepository.GetPersonalInfoById(request.PersonalInfoId);
        if (personalInfo == null)
        {
            return BaseResponse<ProfileImageResponse?>.Fail("La información personal no existe.", 404);
        }

        var userIdClaim = UserContextHelper.GetCurrentUserId(_httpContextAccessor);

        // Convertir el claim a Guid y comparar con PersonalInfoId
        if (!Guid.TryParse(userIdClaim, out var userId) || request.PersonalInfoId != userId)
        {
            return BaseResponse<ProfileImageResponse?>.Fail("No autorizado.", 401);
        }

        // Validar la imagen de perfil
        if (request.ProfileImage == null)
        {
            return BaseResponse<ProfileImageResponse?>.Fail("No se proporcionó ninguna imagen de perfil.", 400);
        }

        try
        {
            ValidateProfileImage(request.ProfileImage);
        }
        catch (ArgumentException ex)
        {
            return BaseResponse<ProfileImageResponse?>.Fail(ex.Message, 400);
        }

        // Manejar la subida de la imagen de perfil
        string? profileImagePath = await HandleProfileImage(request, personalInfo);
        if (profileImagePath == null)
        {
            return BaseResponse<ProfileImageResponse?>.Fail("No se pudo guardar la imagen de perfil.", 500);
        }

        // Actualizar la URL de la imagen en el PersonalInfo
        personalInfo.ProfilePhotoUrl = profileImagePath;
        bool updateResult = await _personalInfoRepository.UpdateProfilePhoto(personalInfo);
        if (!updateResult)
        {
            return BaseResponse<ProfileImageResponse?>.Fail("No se pudo actualizar la información personal con la URL de la imagen.", 500);
        }

        // Crear la respuesta con la URL de la imagen
        var profileImageResponse = new ProfileImageResponse
        {
            ProfilePhotoUrl = profileImagePath
        };

        return BaseResponse<ProfileImageResponse?>.Success(profileImageResponse, "Imagen de perfil subida exitosamente.");
    }

    #region Método Auxiliar

    private async Task<string?> HandleProfileImage(ProfileImageSaveRequest request, PersonalInfo personalInfo)
    {
        if (request.ProfileImage != null)
        {
            // Guardar la imagen en el sistema de archivos
            string savedFileName = await _sftpFileService.UploadFile(request.ProfileImage, _imageProfileFolder);
            return savedFileName;
        }

        return null;
    }

    private bool ValidateProfileImage(IFormFile file)
    {
        // Validar tamaño máximo
        if (file.Length > _maxProfileImageSize)
            throw new ArgumentException($"La imagen excede el tamaño máximo permitido de {_maxProfileImageSize / (1024 * 1024)} MB.");

        // Validar extensiones permitidas
        var allowedImageExtensions = new[] { ".jpg", ".png", ".jpeg" };
        string fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedImageExtensions.Contains(fileExtension))
            throw new ArgumentException($"La extensión '{fileExtension}' no está permitida para imágenes.");

        // Validar contenido del archivo según su tipo
        if (!FileValidationHelper.IsValidFileContent(file, fileExtension))
            throw new ArgumentException($"El archivo no es un {fileExtension} válido.");

        return true;
    }

    #endregion
}