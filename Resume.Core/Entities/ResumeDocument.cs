namespace Resume.Core.Entities;

/// <summary>
/// Representa un documento asociado a un currículum.
/// </summary>
public class ResumeDocument : AuditableEntity
{
    public Guid Id { get; set; }
    public Guid? ResumeId { get; set; }
    public string? DocumentUrl { get; set; }
    public string Title { get; set; }
    public int DocumentTypeId { get; set; }
}
