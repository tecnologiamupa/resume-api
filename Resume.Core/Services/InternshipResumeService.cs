using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;
using Resume.Core.Helpers;
using Resume.Core.RepositoryContracts;
using Resume.Core.ServiceContracts;

namespace Resume.Core.Services;

internal class InternshipResumeService : IInternshipResumeService
{
    private readonly IInternshipResumeRepository _repository;
    private readonly IMapper _mapper;

    public InternshipResumeService(IInternshipResumeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<InternshipResumeResponse?>> GetInternshipResumeById(Guid id)
    {
        var entity = await _repository.GetInternshipResumeById(id);
        if (entity == null)
            return BaseResponse<InternshipResumeResponse?>.Fail("Currículum de pasantía no encontrado", 404);

        var response = _mapper.Map<InternshipResumeResponse>(entity);
        return BaseResponse<InternshipResumeResponse?>.Success(response);
    }

    public async Task<BaseResponse<InternshipResumeResponse?>> GetInternshipResumeByResumeId(Guid resumeId)
    {
        var entity = await _repository.GetInternshipResumeByResumeId(resumeId);
        if (entity == null)
            return BaseResponse<InternshipResumeResponse?>.Fail("Currículum de pasantía no encontrado", 404);

        var response = _mapper.Map<InternshipResumeResponse>(entity);
        return BaseResponse<InternshipResumeResponse?>.Success(response);
    }

    public async Task<BaseResponse<InternshipResumeResponse>> CreateInternshipResume(InternshipResumeCreateRequest request)
    {
        var entity = _mapper.Map<InternshipResume>(request);
        entity.Id = Guid.NewGuid();
        entity.CreatedDate = DateTimeHelper.GetCurrentDateTime();

        var created = await _repository.CreateInternshipResume(entity);
        if (created == null)
            return BaseResponse<InternshipResumeResponse>.Fail("No se pudo crear el currículum de pasantía", 400);

        var response = _mapper.Map<InternshipResumeResponse>(created);
        return BaseResponse<InternshipResumeResponse>.Success(response, "Currículum de pasantía creado exitosamente.", 201);
    }

    public async Task<BaseResponse<bool>> UpdateInternshipResume(Guid id, InternshipResumeUpdateRequest request)
    {
        var existing = await _repository.GetInternshipResumeById(id);
        if (existing == null)
            return BaseResponse<bool>.Fail("Currículum de pasantía no encontrado", 404);

        if (request.ResumeId != null) existing.ResumeId = request.ResumeId;
        if (request.CareerObjective != null) existing.CareerObjective = request.CareerObjective;

        existing.LastModifiedDate = DateTimeHelper.GetCurrentDateTime();

        var result = await _repository.UpdateInternshipResume(existing);
        return result
            ? BaseResponse<bool>.Success(true)
            : BaseResponse<bool>.Fail("No se pudo actualizar el currículum de pasantía", 400);
    }

    public async Task<BaseResponse<bool>> DeleteInternshipResume(Guid id)
    {
        var result = await _repository.DeleteInternshipResume(id);
        return result
            ? BaseResponse<bool>.Success(true)
            : BaseResponse<bool>.Fail("No se pudo eliminar el currículum de pasantía", 400);
    }
}