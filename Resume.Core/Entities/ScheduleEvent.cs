namespace Resume.Core.Entities;

/// <summary>
/// Representa un evento programado dentro del sistema.
/// </summary>
public class ScheduleEvent
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? EventName { get; set; }
    public int? LimitCount { get; set; }
    public bool IsActive { get; set; }
}
