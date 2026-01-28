using Resume.Core.DTOs;

namespace Resume.Core.Entities;

/// <summary>
/// Representa la información personal de un individuo.
/// </summary>
public class PersonalInfo : AuditableEntity
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
    public int? ProvinceId { get; set; }
    public int? DistrictId { get; set; }
    public int? TownshipId { get; set; }
    public int? GenderId { get; set; }
    public bool? HasDisability { get; set; }
    public int? DisabilityTypeId { get; set; }
    public string? DisabilityDescription { get; set; }
    public int? ScheduleId { get; set; }

    // Propiedades de catálogo solo para uso en memoria
    public GenderResponse? Gender { get; set; } = null;
    public ProvinceResponse? Province { get; set; } = null;
    public DistrictResponse? District { get; set; } = null;
    public TownshipResponse? Township { get; set; } = null;
    public DisabilityTypeResponse? DisabilityType { get; set; } = null;
}
