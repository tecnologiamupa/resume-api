using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.RepositoryContracts;
using Resume.Core.ServiceContracts;

namespace Resume.Core.Services;

/// <summary>
/// Servicio para manejar la lógica de negocio relacionada con los tipos de documentos.
/// </summary>
internal class DocumentTypeService : IDocumentTypeService
{
    private readonly IDocumentTypeRepository _documentTypeRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor de la clase <see cref="DocumentTypeService"/>.
    /// </summary>
    /// <param name="documentTypeRepository">Repositorio para interactuar con los datos de los tipos de documentos.</param>
    /// <param name="mapper">Instancia de <see cref="IMapper"/> para mapear entre entidades y DTOs.</param>
    public DocumentTypeService(IDocumentTypeRepository documentTypeRepository, IMapper mapper)
    {
        _documentTypeRepository = documentTypeRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene la lista de tipos de documentos.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. 
    /// El resultado contiene una respuesta base con una lista de objetos <see cref="DocumentTypeResponse"/>.
    /// </returns>
    public async Task<BaseResponse<List<DocumentTypeResponse?>>> GetDocumentTypes()
    {
        // Obtiene los tipos de documentos desde el repositorio.
        var documentTypes = await _documentTypeRepository.GetDocumentTypes();

        // Mapea los tipos de documentos a objetos de respuesta.
        var responses = _mapper.Map<List<DocumentTypeResponse?>>(documentTypes);

        // Retorna una respuesta exitosa con los datos mapeados.
        return BaseResponse<List<DocumentTypeResponse?>>.Success(responses);
    }

    /// <summary>
    /// Obtiene un tipo de documento por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del tipo de documento.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. 
    /// El resultado contiene una respuesta base con un objeto <see cref="DocumentTypeResponse"/>.
    /// </returns>
    public async Task<BaseResponse<DocumentTypeResponse?>> GetDocumentTypeById(int id)
    {
        var documentType = await _documentTypeRepository.GetDocumentTypeById(id);
        if (documentType == null)
        {
            return BaseResponse<DocumentTypeResponse?>.Fail("Tipo de documento no encontrado.");
        }

        var response = _mapper.Map<DocumentTypeResponse?>(documentType);
        return BaseResponse<DocumentTypeResponse?>.Success(response);
    }
}