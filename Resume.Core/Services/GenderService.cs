using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.RepositoryContracts;
using Resume.Core.ServiceContracts;

namespace Resume.Core.Services;

internal class GenderService : IGenderService
{
    private readonly IGenderRepository _genderRepository;
    private readonly IMapper _mapper;

    public GenderService(IGenderRepository genderRepository, IMapper mapper)
    {
        _genderRepository = genderRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<GenderResponse?>>> GetGenders()
    {
        var genders = await _genderRepository.GetGenders();
        var responses = _mapper.Map<List<GenderResponse?>>(genders);
        return BaseResponse<List<GenderResponse?>>.Success(responses);
    }

    public async Task<BaseResponse<GenderResponse?>> GetGenderById(int id)
    {
        var gender = await _genderRepository.GetGenderById(id);
        if (gender == null)
        {
            return BaseResponse<GenderResponse?>.Fail("Género no encontrado.");
        }

        var response = _mapper.Map<GenderResponse?>(gender);
        return BaseResponse<GenderResponse?>.Success(response);
    }
}