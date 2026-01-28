using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.ServiceContracts;
using Resume.Core.RepositoryContracts;

namespace Resume.Core.Services;

internal class SkillCatalogService : ISkillCatalogService
{
    private readonly ISkillCatalogRepository _skillCatalogRepository;
    private readonly IMapper _mapper;

    public SkillCatalogService(ISkillCatalogRepository skillCatalogRepository, IMapper mapper)
    {
        _skillCatalogRepository = skillCatalogRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<SkillCatalogResponse?>>> GetSkillsCatalog()
    {
        var skills = await _skillCatalogRepository.GetSkillsCatalog();
        var responses = _mapper.Map<List<SkillCatalogResponse?>>(skills);
        return BaseResponse<List<SkillCatalogResponse?>>.Success(responses);
    }

    public async Task<BaseResponse<SkillCatalogResponse?>> GetSkillCatalogById(int id)
    {
        var skill = await _skillCatalogRepository.GetSkillCatalogById(id);
        if (skill == null)
        {
            return BaseResponse<SkillCatalogResponse?>.Fail("Habilidad no encontrada.");
        }

        var response = _mapper.Map<SkillCatalogResponse?>(skill);
        return BaseResponse<SkillCatalogResponse?>.Success(response);
    }
}