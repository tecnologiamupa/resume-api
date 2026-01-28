using AutoMapper;
using Microsoft.AspNetCore.Http;
using Resume.Core.DTOs;
using Resume.Core.Entities;
using Resume.Core.Helpers;
using Resume.Core.RepositoryContracts;
using Resume.Core.ServiceContracts;

namespace Resume.Core.Services;

internal class AcademicEducationService : IAcademicEducationService
{
    private readonly IAcademicEducationRepository _academicEducationRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public AcademicEducationService(
        IAcademicEducationRepository academicEducationRepository, 
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor)
    {
        _academicEducationRepository = academicEducationRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Obtiene todas las entradas de educación académica asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <returns>Una colección de entradas de educación académica.</returns>
    public async Task<BaseResponse<List<AcademicEducationResponse>>> GetAcademicEducationsByProfessionalResumeId(Guid professionalResumeId)
    {
        var academicEducations = await _academicEducationRepository.GetAcademicEducationsByProfessionalResumeId(professionalResumeId);
        var responses = _mapper.Map<List<AcademicEducationResponse>>(academicEducations);
        return BaseResponse<List<AcademicEducationResponse>>.Success(responses);
    }

    /// <summary>
    /// Reemplaza todas las entradas de educación académica asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <param name="academicEducations">Colección de entradas de educación académica a crear.</param>
    /// <returns>True si ambas operaciones fueron exitosas; de lo contrario, false.</returns>
    public async Task<BaseResponse<bool>> ReplaceAcademicEducations(Guid professionalResumeId, List<AcademicEducationCreateRequest> academicEducations)
    {
        var academicEducationEntities = _mapper.Map<List<AcademicEducation>>(academicEducations);

        // Asignar el ProfessionalResumeId a cada entidad
        foreach (var academicEducation in academicEducationEntities)
        {
            academicEducation.ProfessionalResumeId = professionalResumeId;
            academicEducation.CreatedDate = DateTimeHelper.GetCurrentDateTime();
            academicEducation.CreatedBy = UserContextHelper.GetCurrentUserId(_httpContextAccessor);
        }

        var result = await _academicEducationRepository.ReplaceAcademicEducations(professionalResumeId, academicEducationEntities);
        return result
            ? BaseResponse<bool>.Success(true, "Entradas de educación académica reemplazadas exitosamente.")
            : BaseResponse<bool>.Fail("No se pudieron reemplazar las entradas de educación académica.");
    }
}