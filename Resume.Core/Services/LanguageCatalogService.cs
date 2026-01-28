using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.ServiceContracts;
using Resume.Core.RepositoryContracts;

namespace Resume.Core.Services;

internal class LanguageCatalogService : ILanguageCatalogService
{
    private readonly ILanguageCatalogRepository _languageCatalogRepository;
    private readonly IMapper _mapper;

    public LanguageCatalogService(ILanguageCatalogRepository languageCatalogRepository, IMapper mapper)
    {
        _languageCatalogRepository = languageCatalogRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<LanguageCatalogResponse?>>> GetLanguagesCatalog()
    {
        var languages = await _languageCatalogRepository.GetLanguagesCatalog();
        var responses = _mapper.Map<List<LanguageCatalogResponse?>>(languages);
        return BaseResponse<List<LanguageCatalogResponse?>>.Success(responses);
    }

    public async Task<BaseResponse<LanguageCatalogResponse?>> GetLanguageCatalogById(int id)
    {
        var language = await _languageCatalogRepository.GetLanguageCatalogById(id);
        if (language == null)
        {
            return BaseResponse<LanguageCatalogResponse?>.Fail("Idioma no encontrado.");
        }

        var response = _mapper.Map<LanguageCatalogResponse?>(language);
        return BaseResponse<LanguageCatalogResponse?>.Success(response);
    }
}