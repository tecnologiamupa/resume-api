namespace Resume.Core.DTOs;

/// <summary>
/// Representa una solicitud para actualizar la información personal de un usuario.
/// </summary>
public class PersonalInfoUpdateRequest
{
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
}
