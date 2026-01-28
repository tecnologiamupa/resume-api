using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.ServiceContracts;

public interface IScheduleService
{
    Task<BaseResponse<List<ScheduleEvent?>>> GetSchedulesByEventId(int eventId);
    Task<BaseResponse<ScheduleEvent?>> GetScheduleById(int id);
}
