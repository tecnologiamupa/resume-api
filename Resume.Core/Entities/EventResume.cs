namespace Resume.Core.Entities;

/// <summary>
/// Representa la relación entre un currículum y un evento programado.
/// </summary>
public class EventResume
{
    public int Id { get; set; }
    public Guid ResumeId { get; set; }
    public int ScheduleId { get; set; }
    public string AddressDetail { get; set; } = string.Empty;
    public int? EventId { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public string? LastModifiedBy { get; set; }
}