using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;
using Resume.Core.Helpers;
using Resume.Core.RepositoryContracts;
using Resume.Core.ServiceContracts;

namespace Resume.Core.Services;

/// <summary>
/// Servicio para la gestión de habilidades técnicas.
/// </summary>
internal class TechnicalSkillService : ITechnicalSkillService
{
    private readonly ITechnicalSkillRepository _technicalSkillRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="TechnicalSkillService"/>.
    /// </summary>
    /// <param name="technicalSkillRepository">Repositorio de habilidades técnicas.</param>
    /// <param name="mapper">Instancia de AutoMapper.</param>
    public TechnicalSkillService(ITechnicalSkillRepository technicalSkillRepository, IMapper mapper)
    {
        _technicalSkillRepository = technicalSkillRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todas las habilidades técnicas asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <returns>Una colección de habilidades técnicas.</returns>
    public async Task<BaseResponse<List<TechnicalSkillResponse>>> GetTechnicalSkillsByProfessionalResumeId(Guid professionalResumeId)
    {
        var technicalSkills = await _technicalSkillRepository.GetTechnicalSkillsByProfessionalResumeId(professionalResumeId);
        var responses = _mapper.Map<List<TechnicalSkillResponse>>(technicalSkills);
        return BaseResponse<List<TechnicalSkillResponse>>.Success(responses);
    }

    /// <summary>
    /// Reemplaza todas las habilidades técnicas asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <param name="technicalSkills">Colección de habilidades técnicas a crear.</param>
    /// <returns>True si ambas operaciones fueron exitosas; de lo contrario, false.</returns>
    public async Task<BaseResponse<bool>> ReplaceTechnicalSkills(Guid professionalResumeId, List<TechnicalSkillCreateRequest> technicalSkills)
    {
        var technicalSkillEntities = _mapper.Map<List<TechnicalSkill>>(technicalSkills);

        // Asignar el ProfessionalResumeId a cada entidad
        foreach (var technicalSkill in technicalSkillEntities)
        {
            technicalSkill.ProfessionalResumeId = professionalResumeId;
        }

        var result = await _technicalSkillRepository.ReplaceTechnicalSkills(professionalResumeId, technicalSkillEntities);
        return result
            ? BaseResponse<bool>.Success(true, "Habilidades técnicas reemplazadas exitosamente.")
            : BaseResponse<bool>.Fail("No se pudieron reemplazar las habilidades técnicas.");
    }
}