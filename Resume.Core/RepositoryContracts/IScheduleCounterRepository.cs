using Resume.Core.Entities;

namespace Resume.Core.RepositoryContracts;

public interface IScheduleCounterRepository
{
    Task<ScheduleCounter?> GetScheduleCounterById(int id);
}
