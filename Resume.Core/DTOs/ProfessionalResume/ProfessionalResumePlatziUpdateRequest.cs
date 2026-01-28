namespace Resume.Core.DTOs;

/// <summary>
/// DTO para actualizar los campos Platzi de un currículum profesional.
/// </summary>
public class ProfessionalResumePlatziUpdateRequest
{
    public Guid ResumeId { get; set; }
    public string? PlatziCompanyUserId { get; set; }
    public string? PlatziUserId { get; set; }
}