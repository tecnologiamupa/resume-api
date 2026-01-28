using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Resume.Core.DTOs;
using Resume.Core.Entities;
using Resume.Core.Helpers;
using Resume.Core.RepositoryContracts;
using Resume.Core.ServiceContracts;
using Resume.Core.Services;

internal class ResumeDocumentService : IResumeDocumentService
{
    private readonly IResumeDocumentRepository _resumeDocumentRepository;
    private readonly IResumeRepository _resumeRepository;
    private readonly IMapper _mapper;
    private readonly ISftpFileService _sftpFileService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDocumentTypeService _documentTypeService;
    private readonly string _documentFolder;
    private readonly long _maxResumeDocumentSize;
    private readonly int _maxResumeDocuments;

    public ResumeDocumentService(
        IResumeDocumentRepository resumeDocumentRepository,
        IResumeRepository resumeRepository,
        IMapper mapper,
        IConfiguration configuration,
        ISftpFileService sftpFileService,
        IHttpContextAccessor httpContextAccessor,
        IDocumentTypeService documentTypeService)
    {
        _resumeDocumentRepository = resumeDocumentRepository;
        _resumeRepository = resumeRepository;
        _mapper = mapper;
        _documentFolder = configuration["Files:ResumeDocumentFolder"] ?? string.Empty;
        _maxResumeDocumentSize = long.Parse(configuration["Files:MaxResumeDocumentSize"] ?? "5242880"); // 5 MB por defecto
        _maxResumeDocuments = int.Parse(configuration["Files:MaxResumeDocuments"] ?? "3"); // Límite máximo de documentos
        _sftpFileService = sftpFileService;
        _httpContextAccessor = httpContextAccessor;
        _documentTypeService = documentTypeService;

    }

    public async Task<BaseResponse<List<ResumeDocumentResponse?>>> GetResumeDocumentsByResumeId(Guid resumeId)
    {
        // Verificar si el currículum existe
        var resume = await _resumeRepository.GetResumeById(resumeId);
        if (resume == null)
        {
            return BaseResponse<List<ResumeDocumentResponse?>>.Fail("El currículum no existe.", 404);
        }

        //var userIdClaim = UserContextHelper.GetCurrentUserId(_httpContextAccessor);

        //// Convertir el claim a Guid y comparar con PersonalInfoId
        //if (!Guid.TryParse(userIdClaim, out var userId) || resume.PersonalInfoId != userId)
        //{
        //    return BaseResponse<List<ResumeDocumentResponse?>>.Fail("No autorizado.", 401);
        //}

        // Obtener los documentos asociados al currículum
        var documents = await _resumeDocumentRepository.GetResumeDocumentsByResumeId(resumeId);

        // Verificar si no se encontraron documentos
        if (documents == null || !documents.Any())
        {
            return BaseResponse<List<ResumeDocumentResponse?>>.Success(new List<ResumeDocumentResponse?>());
        }

        // Mapear los documentos a la respuesta
        var responses = await MapResumeDocumentsToResponses(documents);

        return BaseResponse<List<ResumeDocumentResponse?>>.Success(responses);
    }

    public async Task<BaseResponse<ResumeDocumentResponse?>> CreateResumeDocument(ResumeDocumentCreateRequest request)
    {
        // Verificar si el currículum existe
        var resume = await _resumeRepository.GetResumeById(request.ResumeId);
        if (resume == null)
        {
            return BaseResponse<ResumeDocumentResponse?>.Fail("El currículum no existe.", 404);
        }

        var userIdClaim = UserContextHelper.GetCurrentUserId(_httpContextAccessor);

        // Convertir el claim a Guid y comparar con PersonalInfoId
        if (!Guid.TryParse(userIdClaim, out var userId) || resume.PersonalInfoId != userId)
        {
            return BaseResponse<ResumeDocumentResponse?>.Fail("No autorizado.", 401);
        }

        // Verificar el número de documentos existentes
        var existingDocuments = await _resumeDocumentRepository.GetResumeDocumentsByResumeId(request.ResumeId);
        if (existingDocuments.Count() >= _maxResumeDocuments) // Usar el límite configurado
        {
            return BaseResponse<ResumeDocumentResponse?>.Fail($"Se ha alcanzado el límite máximo de documentos permitidos ({_maxResumeDocuments}).", 400);
        }

        // Validar el documento
        if (request.File == null)
        {
            return BaseResponse<ResumeDocumentResponse?>.Fail("No se proporcionó ningún documento.", 400);
        }

        try
        {
            ValidateResumeDocument(request.File);
        }
        catch (ArgumentException ex)
        {
            return BaseResponse<ResumeDocumentResponse?>.Fail(ex.Message, 400);
        }

        // Manejar la subida del documento
        string? documentPath = await HandleResumeDocument(request, resume);
        if (documentPath == null)
        {
            return BaseResponse<ResumeDocumentResponse?>.Fail("No se pudo guardar el documento.", 500);
        }

        // Crear el documento en la base de datos
        var resumeDocument = new ResumeDocument
        {
            Id = Guid.NewGuid(),
            ResumeId = request.ResumeId,
            DocumentUrl = documentPath,
            Title = request.Title,
            DocumentTypeId = request.DocumentTypeId,
            CreatedDate = DateTimeHelper.GetCurrentDateTime(),
            CreatedBy = UserContextHelper.GetCurrentUserId(_httpContextAccessor),
        };

        var createdDocument = await _resumeDocumentRepository.CreateResumeDocument(resumeDocument);
        if (createdDocument == null)
        {
            return BaseResponse<ResumeDocumentResponse?>.Fail("No se pudo crear el documento en la base de datos.", 500);
        }

        // Crear la respuesta con la URL del documento
        var response = _mapper.Map<ResumeDocumentResponse>(createdDocument);
        response.DocumentUrl = documentPath;

        return BaseResponse<ResumeDocumentResponse?>.Success(response, "Documento subido exitosamente.");
    }

    public async Task<BaseResponse<bool>> DeleteResumeDocument(Guid id)
    {
        // Verificar si el documento existe
        var existingDocument = await _resumeDocumentRepository.GetResumeDocumentById(id);
        if (existingDocument == null)
        {
            return BaseResponse<bool>.Fail("El documento no existe.", 404);
        }

        // Verificar si el currículum existe
        var resume = await _resumeRepository.GetResumeById(existingDocument.ResumeId!.Value);
        if (resume == null)
        {
            return BaseResponse<bool>.Fail("El currículum no existe.", 404);
        }

        var userIdClaim = UserContextHelper.GetCurrentUserId(_httpContextAccessor);

        // Convertir el claim a Guid y comparar con PersonalInfoId
        if (!Guid.TryParse(userIdClaim, out var userId) || resume.PersonalInfoId != userId)
        {
            return BaseResponse<bool>.Fail("No autorizado.", 401);
        }

        // Intentar eliminar el documento
        var result = await _resumeDocumentRepository.DeleteResumeDocument(id);

        // Verificar si la eliminación fue exitosa
        return result
            ? BaseResponse<bool>.Success(true, "Documento eliminado exitosamente.")
            : BaseResponse<bool>.Fail("No se pudo eliminar el documento.", 400);
    }

    #region Métodos Auxiliares

    private async Task<string?> HandleResumeDocument(ResumeDocumentCreateRequest request, ResumeInfo resume)
    {
        if (request.File != null)
        {
            // Guardar el archivo en el sistema de archivos
            string savedFileName = await _sftpFileService.UploadFile(request.File, _documentFolder);
            return savedFileName;
        }

        return null;
    }

    private bool ValidateResumeDocument(IFormFile file)
    {
        // Validar tamaño máximo
        if (file.Length > _maxResumeDocumentSize)
            throw new ArgumentException($"El documento excede el tamaño máximo permitido de {_maxResumeDocumentSize / (1024 * 1024)} MB.");

        // Validar extensiones permitidas
        var allowedDocumentExtensions = new[] { ".pdf", ".jpg", ".png", ".jpeg" };
        string fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedDocumentExtensions.Contains(fileExtension))
            throw new ArgumentException($"La extensión '{fileExtension}' no está permitida para documentos.");

        // Validar contenido del archivo según su tipo
        if (!FileValidationHelper.IsValidFileContent(file, fileExtension))
            throw new ArgumentException($"El archivo no es un {fileExtension} válido.");

        return true;
    }

    /// <summary>
    /// Mapea un DocumentTypeResponse a partir de un DocumentTypeId.
    /// </summary>
    /// <param name="documentTypeId">Identificador del tipo de documento.</param>
    /// <returns>Instancia de DocumentTypeResponse o null si no existe.</returns>
    private async Task<DocumentTypeResponse?> MapDocumentType(int documentTypeId)
    {
        var response = await _documentTypeService.GetDocumentTypeById(documentTypeId);
        if (!response.IsSuccess || response.Data == null)
        {
            return null;
        }

        return response.Data;
    }

    /// <summary>
    /// Mapea una colección de documentos de currículum a sus respuestas correspondientes.
    /// </summary>
    /// <param name="documents">Colección de documentos de currículum.</param>
    /// <returns>Lista de respuestas de documentos de currículum.</returns>
    private async Task<List<ResumeDocumentResponse?>> MapResumeDocumentsToResponses(IEnumerable<ResumeDocument?> documents)
    {
        var documentResponses = new List<ResumeDocumentResponse?>();

        foreach (var document in documents)
        {
            if (document == null) continue;

            // Mapear el documento base
            var response = _mapper.Map<ResumeDocumentResponse?>(document);

            if (response != null)
            {
                // Mapear el tipo de documento usando MapDocumentType
                response.DocumentType = await MapDocumentType(document.DocumentTypeId);

                documentResponses.Add(response);
            }
        }

        return documentResponses;
    }

    #endregion
}