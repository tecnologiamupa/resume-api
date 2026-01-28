using Resume.Core.Enums;
using Resume.Core.RepositoryContracts;

namespace Resume.Core.Helpers;

public static class ScheduleHelper
{
    public static async Task<int> GetNextScheduleId(IScheduleCounterRepository scheduleCounterRepository)
    {
        var morningCounter = await scheduleCounterRepository.GetScheduleCounterById((int)ScheduleEnum.Morning);
        var afternoonCounter = await scheduleCounterRepository.GetScheduleCounterById((int)ScheduleEnum.Afternoon);

        int morning = morningCounter?.Count ?? 0;
        int afternoon = afternoonCounter?.Count ?? 0;

        if (morning < afternoon)
            return (int)ScheduleEnum.Morning;
        if (afternoon < morning)
            return (int)ScheduleEnum.Afternoon;

        return (int)ScheduleEnum.Morning;
    }
}
