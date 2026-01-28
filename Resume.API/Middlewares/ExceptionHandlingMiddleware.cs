using Resume.Core.DTOs;
using System.Net;

namespace Resume.API.Middlewares;

/// <summary>
/// Middleware para manejar excepciones no controladas en la aplicación.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ExceptionHandlingMiddleware"/>.
    /// </summary>
    /// <param name="next">El siguiente delegado de la solicitud en la canalización.</param>
    /// <param name="logger">El logger para registrar información de errores.</param>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invoca el middleware para manejar la solicitud HTTP.
    /// </summary>
    /// <param name="httpContext">El contexto HTTP actual.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrió una excepción no controlada: {Message}", ex.Message);

            var response = new BaseResponse<string>
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = "Ocurrió un error inesperado.",
                IsSuccess = false
            };

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await httpContext.Response.WriteAsJsonAsync(response);
        }
    }
}

/// <summary>
/// Método de extensión utilizado para agregar el middleware a la canalización de solicitudes HTTP.
/// </summary>
public static class ExceptionHandlingMiddlewareExtensions
{
    /// <summary>
    /// Agrega el middleware de manejo de excepciones a la canalización de solicitudes HTTP.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación.</param>
    /// <returns>El constructor de la aplicación con el middleware agregado.</returns>
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
