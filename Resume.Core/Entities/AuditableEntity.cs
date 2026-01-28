namespace Resume.Core.Entities;

/// <summary>
/// Clase base para entidades que requieren auditoría.
/// </summary>
public abstract class AuditableEntity
{
    public DateTime? CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public string? LastModifiedBy { get; set; }
}
