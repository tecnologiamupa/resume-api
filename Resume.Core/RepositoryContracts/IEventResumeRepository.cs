using Resume.Core.DTOs;
using Resume.Core.Entities;

namespace Resume.Core.RepositoryContracts;

public interface IEventResumeRepository
{
    Task<EventResume?> GetEventResumeByResumeId(Guid resumeId, int eventId);
    Task<(IEnumerable<EventResumeResponse> Items, int TotalRecords)> GetPagedEventResumesByFilter(
    EventResumeFilterRequest filter, int pageNumber, int pageSize);
    Task<EventResume> CreateEventResume(EventResume eventResume);
}
