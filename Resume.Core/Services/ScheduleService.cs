using Resume.Core.DTOs;
using Resume.Core.Entities;
using Resume.Core.RepositoryContracts;
using Resume.Core.ServiceContracts;

namespace Resume.Core.Services;

internal class ScheduleService : IScheduleService
{
    private readonly IScheduleRepository _scheduleRepository;

    public ScheduleService(IScheduleRepository scheduleRepository)
    {
        _scheduleRepository = scheduleRepository;
    }

    public async Task<BaseResponse<ScheduleEvent?>> GetScheduleById(int id)
    {
        var schedule = await _scheduleRepository.GetScheduleById(id);
        if (schedule == null)
        {
            return BaseResponse<ScheduleEvent?>.Fail("Horario no encontrado", 404);
        }
        return BaseResponse<ScheduleEvent?>.Success(schedule);
    }

    public async Task<BaseResponse<List<ScheduleEvent?>>> GetSchedulesByEventId(int eventId)
    {
        var schedules = await _scheduleRepository.GetSchedulesByEventId(eventId);
        return BaseResponse<List<ScheduleEvent?>>.Success(schedules.ToList());
    }
}
