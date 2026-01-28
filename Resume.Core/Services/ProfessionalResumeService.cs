using AutoMapper;
using Microsoft.AspNetCore.Http;
using Resume.Core.DTOs;
using Resume.Core.Entities;
using Resume.Core.Helpers;
using Resume.Core.RepositoryContracts;
using Resume.Core.ServiceContracts;

namespace Resume.Core.Services;

internal class ProfessionalResumeService : IProfessionalResumeService
{
    private readonly IProfessionalResumeRepository _repository;
    private readonly IInternshipTypeService _internshipTypeService;
    private readonly IInadehCourseService _inadehCourseService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public ProfessionalResumeService(
        IProfessionalResumeRepository repository, 
        IMapper mapper,
        IInternshipTypeService internshipTypeService,
        IInadehCourseService inadehCourseService,
        IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _mapper = mapper;
        _internshipTypeService = internshipTypeService;
        _inadehCourseService = inadehCourseService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<BaseResponse<ProfessionalResumeResponse?>> GetProfessionalResumeById(Guid id)
    {
        var entity = await _repository.GetProfessionalResumeById(id);
        if (entity == null)
            return BaseResponse<ProfessionalResumeResponse?>.Fail("Currículum profesional no encontrado", 404);

        var response = _mapper.Map<ProfessionalResumeResponse>(entity);

        // Mapear Internship y Inadeh si están presentes
        response.Internship = await MapInternship(entity.InternshipTypeId);
        response.Inadeh = await MapInadeh(entity.InadehCourseId);

        if (entity.IsPlatziAssigned == true)
        {
            var platziResponse = new PlatziResponse
            {
                IsPlatziAssigned = entity.IsPlatziAssigned,
                PlatziCompanyUserId = entity.PlatziCompanyUserId,
                PlatziUserId = entity.PlatziUserId
            };

            response.Platzi = platziResponse;
        }        

        return BaseResponse<ProfessionalResumeResponse?>.Success(response);
    }

    public async Task<BaseResponse<ProfessionalResumeResponse?>> GetProfessionalResumeByResumeId(Guid resumeId)
    {
        var entity = await _repository.GetProfessionalResumeByResumeId(resumeId);
        if (entity == null)
            return BaseResponse<ProfessionalResumeResponse?>.Fail("Currículum profesional no encontrado", 404);

        var response = _mapper.Map<ProfessionalResumeResponse>(entity);

        // Mapear Internship y Inadeh si están presentes
        response.Internship = await MapInternship(entity.InternshipTypeId);
        response.Inadeh = await MapInadeh(entity.InadehCourseId);

        if (entity.IsPlatziAssigned == true)
        {
            var platziResponse = new PlatziResponse
            {
                IsPlatziAssigned = entity.IsPlatziAssigned,
                PlatziCompanyUserId = entity.PlatziCompanyUserId,
                PlatziUserId = entity.PlatziUserId
            };

            response.Platzi = platziResponse;
        }

        return BaseResponse<ProfessionalResumeResponse?>.Success(response);
    }

    public async Task<BaseResponse<ProfessionalResumeResponse>> CreateProfessionalResume(ProfessionalResumeCreateRequest request)
    {
        var entity = _mapper.Map<ProfessionalResume>(request);
        entity.Id = Guid.NewGuid();
        entity.CreatedDate = DateTimeHelper.GetCurrentDateTime();
        entity.CreatedBy = UserContextHelper.GetCurrentUserId(_httpContextAccessor);

        var created = await _repository.CreateProfessionalResume(entity);
        if (created == null)
            return BaseResponse<ProfessionalResumeResponse>.Fail("No se pudo crear el currículum profesional", 400);

        var response = _mapper.Map<ProfessionalResumeResponse>(created);
        return BaseResponse<ProfessionalResumeResponse>.Success(response, "Currículum profesional creado exitosamente.", 201);
    }

    public async Task<BaseResponse<bool>> UpdateProfessionalResume(Guid id, ProfessionalResumeUpdateRequest request)
    {
        var existing = await _repository.GetProfessionalResumeById(id);
        if (existing == null)
            return BaseResponse<bool>.Fail("Currículum profesional no encontrado", 404);

        if (request.ResumeId != null) existing.ResumeId = request.ResumeId;
        if (request.ProfessionalSummary != null) existing.ProfessionalSummary = request.ProfessionalSummary;
        if (request.IsInternshipCandidate.HasValue) existing.IsInternshipCandidate = request.IsInternshipCandidate.Value;
        if (request.InternshipTypeId.HasValue) existing.InternshipTypeId = request.InternshipTypeId.Value;
        if (request.IsInadehCandidate.HasValue) existing.IsInadehCandidate = request.IsInadehCandidate.Value;
        if (request.InadehCourseId.HasValue) existing.InadehCourseId = request.InadehCourseId.Value;

        existing.LastModifiedDate = DateTimeHelper.GetCurrentDateTime();
        existing.LastModifiedBy = UserContextHelper.GetCurrentUserId(_httpContextAccessor);

        var result = await _repository.UpdateProfessionalResume(existing);
        return result
            ? BaseResponse<bool>.Success(true)
            : BaseResponse<bool>.Fail("No se pudo actualizar el currículum profesional", 400);
    }

    public async Task<BaseResponse<bool>> UpdatePlatziFieldsByResumeListAsync(IEnumerable<ProfessionalResumePlatziUpdateRequest> requests)
    {
        if (requests == null || !requests.Any())
            return BaseResponse<bool>.Fail("La lista de currículums está vacía.", 400);

        var userId = UserContextHelper.GetCurrentUserId(_httpContextAccessor);
        var now = DateTimeHelper.GetCurrentDateTime();

        // Mapear los DTOs a entidades con los campos requeridos
        var entities = requests.Select(r => new ProfessionalResume
        {
            ResumeId = r.ResumeId,
            IsPlatziAssigned = true,
            PlatziCompanyUserId = r.PlatziCompanyUserId,
            PlatziUserId = r.PlatziUserId,
            LastModifiedDate = now,
            LastModifiedBy = userId
        });

        var result = await _repository.UpdatePlatziFieldsByResumeListAsync(entities);
        return result
            ? BaseResponse<bool>.Success(true, "Campos Platzi actualizados correctamente.")
            : BaseResponse<bool>.Fail("No se pudo actualizar los campos Platzi.", 400);
    }

    public async Task<BaseResponse<bool>> DeleteProfessionalResume(Guid id)
    {
        var result = await _repository.DeleteProfessionalResume(id);
        return result
            ? BaseResponse<bool>.Success(true)
            : BaseResponse<bool>.Fail("No se pudo eliminar el currículum profesional", 400);
    }

    #region Método Auxiliar

    // Método para mapear Internship
    private async Task<InternshipResponse?> MapInternship(int? internshipTypeId)
    {
        if (!internshipTypeId.HasValue || internshipTypeId.Value == 0)
            return null;

        var response = await _internshipTypeService.GetInternshipTypeById(internshipTypeId.Value);
        if (!response.IsSuccess || response.Data == null)
            return null;

        return new InternshipResponse
        {
            IsInternshipCandidate = true, // Ajustar según la lógica de negocio
            InternshipType = response.Data
        };
    }

    // Método para mapear Inadeh
    private async Task<InadehResponse?> MapInadeh(int? inadehCourseId)
    {
        if (!inadehCourseId.HasValue || inadehCourseId.Value == 0)
            return null;

        var response = await _inadehCourseService.GetInadehCourseById(inadehCourseId.Value);
        if (!response.IsSuccess || response.Data == null)
            return null;

        return new InadehResponse
        {
            IsInadehCandidate = true, // Ajustar según la lógica de negocio
            InadehCourse = response.Data
        };
    }

    #endregion
}