using AutoMapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;
using Resume.Core.Helpers;
using Resume.Core.RepositoryContracts;
using Resume.Core.ServiceContracts;

namespace Resume.Core.Services;

internal class SportsResumeService : ISportsResumeService
{
    private readonly ISportsResumeRepository _repository;
    private readonly IMapper _mapper;

    public SportsResumeService(ISportsResumeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<SportsResumeResponse?>> GetSportsResumeById(Guid id)
    {
        var entity = await _repository.GetSportsResumeById(id);
        if (entity == null)
            return BaseResponse<SportsResumeResponse?>.Fail("Currículum deportivo no encontrado", 404);

        var response = _mapper.Map<SportsResumeResponse>(entity);
        return BaseResponse<SportsResumeResponse?>.Success(response);
    }

    public async Task<BaseResponse<SportsResumeResponse?>> GetSportsResumeByResumeId(Guid resumeId)
    {
        var entity = await _repository.GetSportsResumeByResumeId(resumeId);
        if (entity == null)
            return BaseResponse<SportsResumeResponse?>.Fail("Currículum deportivo no encontrado", 404);

        var response = _mapper.Map<SportsResumeResponse>(entity);
        return BaseResponse<SportsResumeResponse?>.Success(response);
    }

    public async Task<BaseResponse<SportsResumeResponse>> CreateSportsResume(SportsResumeCreateRequest request)
    {
        var entity = _mapper.Map<SportsResume>(request);
        entity.Id = Guid.NewGuid();
        entity.CreatedDate = DateTimeHelper.GetCurrentDateTime();

        var created = await _repository.CreateSportsResume(entity);
        if (created == null)
            return BaseResponse<SportsResumeResponse>.Fail("No se pudo crear el currículum deportivo", 400);

        var response = _mapper.Map<SportsResumeResponse>(created);
        return BaseResponse<SportsResumeResponse>.Success(response, "Currículum deportivo creado exitosamente.", 201);
    }

    public async Task<BaseResponse<bool>> UpdateSportsResume(Guid id, SportsResumeUpdateRequest request)
    {
        var existing = await _repository.GetSportsResumeById(id);
        if (existing == null)
            return BaseResponse<bool>.Fail("Currículum deportivo no encontrado", 404);

        if (request.ResumeId != null) existing.ResumeId = request.ResumeId;
        if (request.SportsSummary != null) existing.SportsSummary = request.SportsSummary;

        existing.LastModifiedDate = DateTimeHelper.GetCurrentDateTime();

        var result = await _repository.UpdateSportsResume(existing);
        return result
            ? BaseResponse<bool>.Success(true)
            : BaseResponse<bool>.Fail("No se pudo actualizar el currículum deportivo", 400);
    }

    public async Task<BaseResponse<bool>> DeleteSportsResume(Guid id)
    {
        var result = await _repository.DeleteSportsResume(id);
        return result
            ? BaseResponse<bool>.Success(true)
            : BaseResponse<bool>.Fail("No se pudo eliminar el currículum deportivo", 400);
    }
}