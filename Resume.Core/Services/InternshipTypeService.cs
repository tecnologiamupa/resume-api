using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.RepositoryContracts;
using Resume.Core.ServiceContracts;

namespace Resume.Core.Services;

/// <summary>
/// Servicio para gestionar los tipos de pasantías.
/// </summary>
internal class InternshipTypeService : IInternshipTypeService
{
    private readonly IInternshipTypeRepository _internshipTypeRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor de la clase <see cref="InternshipTypeService"/>.
    /// </summary>
    /// <param name="internshipTypeRepository">Repositorio para interactuar con los tipos de pasantías.</param>
    /// <param name="mapper">Instancia de <see cref="IMapper"/> para mapear entidades.</param>
    public InternshipTypeService(IInternshipTypeRepository internshipTypeRepository, IMapper mapper)
    {
        _internshipTypeRepository = internshipTypeRepository;
        _mapper = mapper;
    }    

    /// <summary>
    /// Obtiene una lista de tipos de pasantías.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El resultado contiene una respuesta base con una lista de objetos <see cref="InternshipTypeResponse"/>.
    /// </returns>
    public async Task<BaseResponse<List<InternshipTypeResponse?>>> GetInternshipTypes()
    {
        var internshipTypes = await _internshipTypeRepository.GetInternshipTypes();
        var responses = _mapper.Map<List<InternshipTypeResponse?>>(internshipTypes);
        return BaseResponse<List<InternshipTypeResponse?>>.Success(responses);
    }

    public async Task<BaseResponse<InternshipTypeResponse?>> GetInternshipTypeById(int id)
    {
        var internshipType = await _internshipTypeRepository.GetInternshipTypeById(id);
        if (internshipType == null)
        {
            return BaseResponse<InternshipTypeResponse?>.Fail("Tipo de pasantía no encontrado.");
        }
        
        var response = _mapper.Map<InternshipTypeResponse?>(internshipType);
        return BaseResponse<InternshipTypeResponse?>.Success(response);
    }
}
