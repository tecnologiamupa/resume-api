using Microsoft.AspNetCore.Http;
using Resume.Core.Entities;
using System.Text.Json;

namespace Resume.Core.Helpers;

public static class ApiLogHelper
{
    public static ApiLog MapFromRequest(HttpContext httpContext, object? requestBody, string message, string level = "Information", string? exception = null)
    {
        return new ApiLog
        {
            Timestamp = DateTimeHelper.GetCurrentDateTime(),
            Level = level,
            Message = message,
            Exception = exception,
            RequestPath = httpContext.Request.Path,
            HttpMethod = httpContext.Request.Method,
            IpAddress = httpContext.Connection.RemoteIpAddress?.ToString(),
            UserAgent = httpContext.Request.Headers["User-Agent"].ToString(),
            Referer = httpContext.Request.Headers["Referer"].ToString(),
            RequestBody = requestBody != null ? JsonSerializer.Serialize(requestBody) : null
        };
    }
}
