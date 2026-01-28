using AutoMapper;
using Microsoft.AspNetCore.Http;
using Resume.Core.DTOs;
using Resume.Core.Entities;
using Resume.Core.Helpers;
using Resume.Core.RepositoryContracts;
using Resume.Core.ServiceContracts;
using Sigueme.Core.DTOs;

namespace Resume.Core.Services;

internal class EventResumeService : IEventResumeService
{
    private readonly IEventResumeRepository _eventResumeRepository;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IScheduleCounterRepository _scheduleCounterRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EventResumeService(
        IEventResumeRepository eventResumeRepository,
        IScheduleRepository scheduleRepository,
        IScheduleCounterRepository scheduleCounterRepository,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor)
    {
        _eventResumeRepository = eventResumeRepository;
        _scheduleRepository = scheduleRepository;
        _scheduleCounterRepository = scheduleCounterRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<BaseResponse<EventResume?>> GetEventResumeByResumeId(Guid resumeId, int eventId)
    {
        var eventResume = await _eventResumeRepository.GetEventResumeByResumeId(resumeId, eventId);
        if (eventResume == null)
            return BaseResponse<EventResume?>.Fail("No se encontró la relación evento-currículum", 404);

        var response = _mapper.Map<EventResume>(eventResume);
        return BaseResponse<EventResume?>.Success(response);
    }

    public async Task<PaginatedResponse<List<EventResumeResponse>>> GetPagedEventResumesByFilter(
    EventResumeFilterRequest filter, int pageNumber, int pageSize)
    {
        // Llama al repositorio para obtener los registros y el total
        var (items, totalRecords) = await _eventResumeRepository.GetPagedEventResumesByFilter(filter, pageNumber, pageSize);

        return PaginatedResponse<List<EventResumeResponse>>.Success(
            items.ToList(),
            totalRecords,
            pageNumber,
            pageSize
        );
    }

    public async Task<BaseResponse<EventResume>> CreateEventResume(EventResumeCreateRequest eventResumeRequest)
    {
        // Validar si ya existe la relación evento-currículum
        var existing = await _eventResumeRepository.GetEventResumeByResumeId(eventResumeRequest.ResumeId, eventResumeRequest.EventId);
        if (existing != null)
        {
            return BaseResponse<EventResume>.Fail("Ya ha seleccionado un horario para este evento.", 400);
        }

        // Obtener el horario y su límite
        var schedule = await _scheduleRepository.GetScheduleById(eventResumeRequest.ScheduleId);
        if (schedule == null)
        {
            return BaseResponse<EventResume>.Fail("El horario seleccionado no existe.", 404);
        }

        // Validar antes de crear
        var scheduleCounterBefore = await _scheduleCounterRepository.GetScheduleCounterById(eventResumeRequest.ScheduleId);
        int currentCountBefore = scheduleCounterBefore?.Count ?? 0;
        int limitCount = schedule.LimitCount ?? 0;

        if (limitCount > 0 && currentCountBefore >= limitCount)
        {
            return BaseResponse<EventResume>.Fail("El horario seleccionado ya alcanzó el límite de inscripciones.", 409);
        }

        var eventResume = _mapper.Map<EventResume>(eventResumeRequest);
        eventResume.CreatedDate = DateTimeHelper.GetCurrentDateTime();
        eventResume.CreatedBy = UserContextHelper.GetCurrentUserId(_httpContextAccessor);

        var created = await _eventResumeRepository.CreateEventResume(eventResume);
        if (created == null)
            return BaseResponse<EventResume>.Fail("No se pudo crear la relación evento-currículum", 400);

        // Validar después de crear (por si justo se alcanzó el tope)
        var scheduleCounterAfter = await _scheduleCounterRepository.GetScheduleCounterById(eventResumeRequest.ScheduleId);
        int currentCountAfter = scheduleCounterAfter?.Count ?? 0;

        if (limitCount > 0 && currentCountAfter >= limitCount && schedule.IsActive)
        {
            schedule.IsActive = false;
            await _scheduleRepository.UpdateSchedule(schedule);
        }

        var response = _mapper.Map<EventResume>(created);
        return BaseResponse<EventResume>.Success(response, "Relación evento-currículum creada exitosamente.", 201);
    }
}