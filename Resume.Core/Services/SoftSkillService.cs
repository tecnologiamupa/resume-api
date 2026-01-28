using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;
using Resume.Core.Helpers;
using Resume.Core.RepositoryContracts;
using Resume.Core.ServiceContracts;

namespace Resume.Core.Services;

/// <summary>
/// Servicio para la gestión de habilidades blandas.
/// </summary>
internal class SoftSkillService : ISoftSkillService
{
    private readonly ISoftSkillRepository _softSkillRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="SoftSkillService"/>.
    /// </summary>
    /// <param name="softSkillRepository">Repositorio de habilidades blandas.</param>
    /// <param name="mapper">Instancia de AutoMapper.</param>
    public SoftSkillService(ISoftSkillRepository softSkillRepository, IMapper mapper)
    {
        _softSkillRepository = softSkillRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todas las habilidades blandas asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <returns>Una colección de habilidades blandas.</returns>
    public async Task<BaseResponse<List<SoftSkillResponse>>> GetSoftSkillsByProfessionalResumeId(Guid professionalResumeId)
    {
        var softSkills = await _softSkillRepository.GetSoftSkillsByProfessionalResumeId(professionalResumeId);
        var responses = _mapper.Map<List<SoftSkillResponse>>(softSkills);
        return BaseResponse<List<SoftSkillResponse>>.Success(responses);
    }

    /// <summary>
    /// Reemplaza todas las habilidades blandas asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <param name="softSkills">Colección de habilidades blandas a crear.</param>
    /// <returns>True si ambas operaciones fueron exitosas; de lo contrario, false.</returns>
    public async Task<BaseResponse<bool>> ReplaceSoftSkills(Guid professionalResumeId, List<SoftSkillCreateRequest> softSkills)
    {
        var softSkillEntities = _mapper.Map<List<SoftSkill>>(softSkills);

        // Asignar el ProfessionalResumeId a cada entidad
        foreach (var softSkill in softSkillEntities)
        {
            softSkill.ProfessionalResumeId = professionalResumeId;
        }

        var result = await _softSkillRepository.ReplaceSoftSkills(professionalResumeId, softSkillEntities);
        return result
            ? BaseResponse<bool>.Success(true, "Habilidades blandas reemplazadas exitosamente.")
            : BaseResponse<bool>.Fail("No se pudieron reemplazar las habilidades blandas.");
    }
}