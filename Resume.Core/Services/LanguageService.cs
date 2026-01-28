using AutoMapper;
using Microsoft.AspNetCore.Http;
using Resume.Core.DTOs;
using Resume.Core.Entities;
using Resume.Core.Helpers;
using Resume.Core.RepositoryContracts;
using Resume.Core.ServiceContracts;

namespace Resume.Core.Services;

/// <summary>
/// Servicio para la gestión de idiomas.
/// </summary>
internal class LanguageService : ILanguageService
{
    private readonly ILanguageRepository _languageRepository;
    private readonly IMapper _mapper;
    private readonly ILanguageCatalogService _languageCatalogService;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="LanguageService"/>.
    /// </summary>
    /// <param name="languageRepository">Repositorio de idiomas.</param>
    /// <param name="mapper">Instancia de AutoMapper.</param>
    public LanguageService(
        ILanguageRepository languageRepository, 
        IMapper mapper,
        ILanguageCatalogService languageCatalogService)
    {
        _languageRepository = languageRepository;
        _mapper = mapper;
        _languageCatalogService = languageCatalogService;
    }

    /// <summary>
    /// Obtiene todos los idiomas asociados a una información personal.
    /// </summary>
    /// <param name="personalInfoId">Identificador de la información personal.</param>
    /// <returns>Una colección de idiomas.</returns>
    public async Task<BaseResponse<List<LanguageResponse?>>> GetLanguagesByPersonalInfoId(Guid personalInfoId)
    {
        var languages = await _languageRepository.GetLanguagesByPersonalInfoId(personalInfoId);
        //var responses = _mapper.Map<List<LanguageResponse>>(languages);

        var responses = await MapPersonalLanguagesToResponses(languages);
        return BaseResponse<List<LanguageResponse?>>.Success(responses);
    }

    /// <summary>
    /// Reemplaza todos los idiomas asociados a una información personal.
    /// </summary>
    /// <param name="personalInfoId">Identificador de la información personal.</param>
    /// <param name="languages">Colección de idiomas a crear.</param>
    /// <returns>True si ambas operaciones fueron exitosas; de lo contrario, false.</returns>
    public async Task<BaseResponse<bool>> ReplaceLanguages(Guid personalInfoId, List<LanguageCreateRequest> languages)
    {
        var languageEntities = _mapper.Map<List<PersonalLanguage>>(languages);

        // Asignar el PersonalInfoId a cada entidad
        foreach (var language in languageEntities)
        {
            language.PersonalInfoId = personalInfoId;
        }

        var result = await _languageRepository.ReplaceLanguages(personalInfoId, languageEntities);
        return result
            ? BaseResponse<bool>.Success(true, "Idiomas reemplazados exitosamente.")
            : BaseResponse<bool>.Fail("No se pudieron reemplazar los idiomas.");
    }

    #region Métodos Auxiliares

    /// <summary>
    /// Mapea el catálogo de idioma a partir de su identificador.
    /// </summary>
    /// <param name="languageCatalogId">Identificador del catálogo de idioma.</param>
    /// <returns>Una instancia de <see cref="LanguageCatalogResponse"/> si existe; de lo contrario, <c>null</c>.</returns>
    private async Task<LanguageCatalogResponse?> MapLanguageCatalog(int? languageCatalogId)
    {
        if (languageCatalogId == null) return null;

        var response = await _languageCatalogService.GetLanguageCatalogById(languageCatalogId.Value);
        if (!response.IsSuccess || response.Data == null)
        {
            return null;
        }

        return response.Data;
    }

    /// <summary>
    /// Mapea una colección de idiomas personales a sus respuestas correspondientes, incluyendo el catálogo de idioma.
    /// </summary>
    /// <param name="personalLanguages">Colección de idiomas personales.</param>
    /// <returns>Lista de respuestas de idiomas.</returns>
    private async Task<List<LanguageResponse?>> MapPersonalLanguagesToResponses(IEnumerable<PersonalLanguage?> personalLanguages)
    {
        var languageResponses = new List<LanguageResponse?>();

        foreach (var personalLanguage in personalLanguages)
        {
            if (personalLanguage == null) continue;

            // Mapear la información base del idioma
            var response = _mapper.Map<LanguageResponse?>(personalLanguage);

            if (response != null)
            {
                // Mapear el catálogo de idioma usando MapLanguageCatalog
                response.LanguageCatalog = await MapLanguageCatalog(personalLanguage.LanguageId);

                languageResponses.Add(response);
            }
        }

        return languageResponses;
    }

    #endregion
}