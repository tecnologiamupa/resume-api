namespace Resume.Core.DTOs;

public class EventResumeResponse
{
    public int Id { get; set; }

    // ResumeInfo/PersonalInfo fields
    public Guid ResumeId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string IdentityNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneCountryCode { get; set; } = string.Empty;
    public string Mobile { get; set; } = string.Empty;


    // ScheduleEvent field
    public int ScheduleId { get; set; }
    public string ScheduleName { get; set; } = string.Empty;

    public string AddressDetail { get; set; } = string.Empty;
    public int? EventId { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public string? LastModifiedBy { get; set; }    
}
