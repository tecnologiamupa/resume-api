using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;
using Resume.Core.RepositoryContracts;
using Resume.Core.ServiceContracts;

namespace Resume.Core.Services;

internal class ProfessionalSkillService : IProfessionalSkillService
{
    private readonly IProfessionalSkillRepository _professionalSkillRepository;
    private readonly ISkillCatalogService _skillCatalogService;
    private readonly IMapper _mapper;

    public ProfessionalSkillService(
        IProfessionalSkillRepository professionalSkillRepository,
        ISkillCatalogService skillCatalogService,
        IMapper mapper)
    {
        _professionalSkillRepository = professionalSkillRepository;
        _skillCatalogService = skillCatalogService;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todas las habilidades profesionales asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <returns>Una colección de habilidades profesionales.</returns>
    public async Task<BaseResponse<List<ProfessionalSkillResponse>>> GetSkillsByProfessionalResumeId(Guid professionalResumeId)
    {
        var skills = await _professionalSkillRepository.GetSkillsByProfessionalResumeId(professionalResumeId);
        var responses = await MapProfessionalSkillsToResponses(skills);
        return BaseResponse<List<ProfessionalSkillResponse>>.Success(responses);
    }

    /// <summary>
    /// Reemplaza todas las habilidades profesionales asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <param name="professionalSkills">Colección de habilidades profesionales a crear.</param>
    /// <returns>True si ambas operaciones fueron exitosas; de lo contrario, false.</returns>
    public async Task<BaseResponse<bool>> ReplaceProfessionalSkills(Guid professionalResumeId, List<ProfessionalSkillCreateRequest> professionalSkills)
    {
        var skillEntities = _mapper.Map<List<ProfessionalSkill>>(professionalSkills);

        // Asignar el ProfessionalResumeId a cada entidad
        foreach (var skill in skillEntities)
        {
            skill.ProfessionalResumeId = professionalResumeId;
        }

        var result = await _professionalSkillRepository.ReplaceProfessionalSkills(professionalResumeId, skillEntities);
        return result
            ? BaseResponse<bool>.Success(true, "Habilidades profesionales reemplazadas exitosamente.")
            : BaseResponse<bool>.Fail("No se pudieron reemplazar las habilidades profesionales.");
    }

    #region Métodos Auxiliares

    /// <summary>
    /// Mapea el catálogo de habilidad a partir de su identificador.
    /// </summary>
    /// <param name="skillCatalogId">Identificador del catálogo de habilidad.</param>
    /// <returns>Una instancia de <see cref="SkillCatalogResponse"/> si existe; de lo contrario, <c>null</c>.</returns>
    private async Task<SkillCatalogResponse?> MapSkillCatalog(int? skillCatalogId)
    {
        if (skillCatalogId == null) return null;

        var response = await _skillCatalogService.GetSkillCatalogById(skillCatalogId.Value);
        if (!response.IsSuccess || response.Data == null)
        {
            return null;
        }

        return response.Data;
    }

    /// <summary>
    /// Mapea una colección de habilidades profesionales a sus respuestas correspondientes, incluyendo el catálogo de habilidad.
    /// </summary>
    /// <param name="professionalSkills">Colección de habilidades profesionales.</param>
    /// <returns>Lista de respuestas de habilidades profesionales.</returns>
    private async Task<List<ProfessionalSkillResponse>> MapProfessionalSkillsToResponses(IEnumerable<ProfessionalSkill?> professionalSkills)
    {
        var skillResponses = new List<ProfessionalSkillResponse>();

        foreach (var professionalSkill in professionalSkills)
        {
            if (professionalSkill == null) continue;

            var response = _mapper.Map<ProfessionalSkillResponse>(professionalSkill);

            if (response != null)
            {
                response.SkillCatalog = await MapSkillCatalog(professionalSkill.SkillId);
                skillResponses.Add(response);
            }
        }

        return skillResponses;
    }

    #endregion
}