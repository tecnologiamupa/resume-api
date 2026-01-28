using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.RepositoryContracts;
using Resume.Core.ServiceContracts;

namespace Resume.Core.Services;

internal class DisabilityTypeService : IDisabilityTypeService
{
    private readonly IDisabilityTypeRepository _disabilityTypeRepository;
    private readonly IMapper _mapper;

    public DisabilityTypeService(IDisabilityTypeRepository disabilityTypeRepository, IMapper mapper)
    {
        _disabilityTypeRepository = disabilityTypeRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<DisabilityTypeResponse?>>> GetDisabilityTypes()
    {
        var disabilityTypes = await _disabilityTypeRepository.GetDisabilityTypes();
        var responses = _mapper.Map<List<DisabilityTypeResponse?>>(disabilityTypes);
        return BaseResponse<List<DisabilityTypeResponse?>>.Success(responses);
    }

    public async Task<BaseResponse<DisabilityTypeResponse?>> GetDisabilityTypeById(int id)
    {
        var disabilityType = await _disabilityTypeRepository.GetDisabilityTypeById(id);
        if (disabilityType == null)
        {
            return BaseResponse<DisabilityTypeResponse?>.Fail("Tipo de discapacidad no encontrado.");
        }

        var response = _mapper.Map<DisabilityTypeResponse?>(disabilityType);
        return BaseResponse<DisabilityTypeResponse?>.Success(response);
    }
}