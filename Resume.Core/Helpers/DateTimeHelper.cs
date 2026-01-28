namespace Resume.Core.Helpers;

/// <summary>
/// Proporciona métodos auxiliares para trabajar con fechas y horas.
/// </summary>
public static class DateTimeHelper
{
    /// <summary>
    /// Obtiene la fecha y hora actual.
    /// </summary>
    /// <returns>La fecha y hora actual.</returns>
    public static DateTime GetCurrentDateTime()
    {
        TimeZoneInfo panamaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Panama");
        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, panamaTimeZone);
    }
}
