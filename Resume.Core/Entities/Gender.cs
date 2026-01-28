namespace Resume.Core.Entities;

/// <summary>
/// Representa un género que puede ser asociado a una entidad.
/// </summary>
public class Gender
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
