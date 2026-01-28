using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Resume.Core.Helpers;

public static class UserContextHelper
{
    /// <summary>
    /// Obtiene el identificador del usuario actual desde el contexto HTTP.
    /// </summary>
    /// <param name="httpContextAccessor">Accesor del contexto HTTP.</param>
    /// <returns>El identificador del usuario actual o null si no está disponible.</returns>
    public static string? GetCurrentUserId(IHttpContextAccessor httpContextAccessor)
    {
        var user = httpContextAccessor.HttpContext?.User;
        return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
