using Resume.Core.DTOs;
using Resume.Core.Entities;
using Sigueme.Core.DTOs;

namespace Resume.Core.ServiceContracts;

public interface IEventResumeService
{
    Task<BaseResponse<EventResume?>> GetEventResumeByResumeId(Guid resumeId, int eventId);
    Task<PaginatedResponse<List<EventResumeResponse>>> GetPagedEventResumesByFilter(
    EventResumeFilterRequest filter, int pageNumber, int pageSize);
    Task<BaseResponse<EventResume>> CreateEventResume(EventResumeCreateRequest eventResumeRequest);
}
