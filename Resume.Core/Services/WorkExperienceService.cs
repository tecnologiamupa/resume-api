using AutoMapper;
using Microsoft.AspNetCore.Http;
using Resume.Core.DTOs;
using Resume.Core.Entities;
using Resume.Core.Helpers;
using Resume.Core.RepositoryContracts;
using Resume.Core.ServiceContracts;

namespace Resume.Core.Services;

internal class WorkExperienceService : IWorkExperienceService
{
    private readonly IWorkExperienceRepository _workExperienceRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WorkExperienceService(
        IWorkExperienceRepository workExperienceRepository, 
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor)
    {
        _workExperienceRepository = workExperienceRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Obtiene todas las experiencias laborales asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <returns>Una colección de experiencias laborales.</returns>
    public async Task<BaseResponse<List<WorkExperienceResponse>>> GetWorkExperiencesByProfessionalResumeId(Guid professionalResumeId)
    {
        var workExperiences = await _workExperienceRepository.GetWorkExperiencesByProfessionalResumeId(professionalResumeId);
        var responses = _mapper.Map<List<WorkExperienceResponse>>(workExperiences);
        return BaseResponse<List<WorkExperienceResponse>>.Success(responses);
    }

    /// <summary>
    /// Reemplaza todas las experiencias laborales asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <param name="workExperiences">Colección de experiencias laborales a crear.</param>
    /// <returns>True si ambas operaciones fueron exitosas; de lo contrario, false.</returns>
    public async Task<BaseResponse<bool>> ReplaceWorkExperiences(Guid professionalResumeId, List<WorkExperienceCreateRequest> workExperiences)
    {
        var workExperienceEntities = _mapper.Map<List<WorkExperience>>(workExperiences);

        // Asignar el ProfessionalResumeId a cada entidad
        foreach (var workExperience in workExperienceEntities)
        {
            workExperience.ProfessionalResumeId = professionalResumeId;
            workExperience.CreatedDate = DateTimeHelper.GetCurrentDateTime();
            workExperience.CreatedBy = UserContextHelper.GetCurrentUserId(_httpContextAccessor);
        }

        var result = await _workExperienceRepository.ReplaceWorkExperiences(professionalResumeId, workExperienceEntities);
        return result
            ? BaseResponse<bool>.Success(true, "Experiencias laborales reemplazadas exitosamente.")
            : BaseResponse<bool>.Fail("No se pudieron reemplazar las experiencias laborales.");
    }
}