namespace Resume.Core.DTOs;

/// <summary>
/// Representa una solicitud para crear un currículum.
/// </summary>
public class ResumeCreateRequest
{
    public int ResumeTypeId { get; set; }
    public string? LinkedIn { get; set; }
    public string? PortfolioUrl { get; set; }
    public string? Summary { get; set; }
}
