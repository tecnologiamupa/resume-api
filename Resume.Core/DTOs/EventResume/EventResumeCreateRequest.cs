namespace Resume.Core.DTOs;

public class EventResumeCreateRequest
{
    public Guid ResumeId { get; set; }
    public int ScheduleId { get; set; }
    public string AddressDetail { get; set; } = string.Empty;
    public int EventId { get; set; }
}
