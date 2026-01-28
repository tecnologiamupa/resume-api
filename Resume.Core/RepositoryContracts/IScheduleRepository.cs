using Resume.Core.Entities;

namespace Resume.Core.RepositoryContracts;

public interface IScheduleRepository
{
    Task<IEnumerable<ScheduleEvent?>> GetSchedulesByEventId(int eventId);
    Task<ScheduleEvent?> GetScheduleById(int id);
    Task<ScheduleEvent> UpdateSchedule(ScheduleEvent scheduleEvent);
}
