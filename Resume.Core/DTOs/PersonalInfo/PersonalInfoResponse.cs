using Resume.Core.Entities;

namespace Resume.Core.DTOs;

/// <summary>
/// Representa la respuesta con la información personal de un usuario.
/// </summary>
public class PersonalInfoResponse
{
    public Guid Id { get; set; }
    public string? ProfilePhotoUrl { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? IdentityNumber { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Title { get; set; }
    public string? Email { get; set; }
    public string? PhoneCountryCode { get; set; }
    public string? Mobile { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public ProvinceResponse? Province { get; set; } = null;
    public DistrictResponse? District { get; set; } = null;
    public TownshipResponse? Township { get; set; } = null;
    public GenderResponse? Gender { get; set; }
    public DisabilityTypeResponse? DisabilityType { get; set; }
    public bool? HasDisability { get; set; }
    public string? DisabilityDescription { get; set; }
    public int? ScheduleId { get; set; }
}
