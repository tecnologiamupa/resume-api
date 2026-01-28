namespace Resume.Core.DTOs;

public class ResumeDocumentResponse
{
    public Guid Id { get; set; }
    public Guid? ResumeId { get; set; }
    public string Title { get; set; }
    public DocumentTypeResponse? DocumentType { get; set; } = null;
    public string? DocumentUrl { get; set; }
    public DateTime? CreatedDate { get; set; }
}
