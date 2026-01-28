using Microsoft.AspNetCore.Http;

namespace Resume.Core.DTOs;

public class ResumeDocumentCreateRequest
{
    public Guid ResumeId { get; set; }
    public string Title { get; set; }
    public int DocumentTypeId { get; set; }
    public IFormFile File { get; set; }
}
