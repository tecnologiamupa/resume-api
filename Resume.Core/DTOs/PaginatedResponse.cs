using Resume.Core.DTOs;

namespace Sigueme.Core.DTOs;

/// <summary>
/// Clase para respuestas de API con soporte de paginación.
/// </summary>
/// <typeparam name="T">Tipo de dato de la respuesta.</typeparam>
public class PaginatedResponse<T> : BaseResponse<T>
{
    public int TotalRecords { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }

    /// <summary>
    /// Constructor para respuestas paginadas exitosas.
    /// </summary>
    /// <param name="data">Datos de la respuesta.</param>
    /// <param name="totalRecords">Total de registros disponibles.</param>
    /// <param name="currentPage">Número de la página actual.</param>
    /// <param name="pageSize">Tamaño de la página.</param>
    /// <param name="message">Mensaje de la respuesta.</param>
    /// <param name="statusCode">Código de estado HTTP.</param>
    /// <returns>Una instancia de <see cref="PaginatedResponse{T}"/> con los datos de éxito.</returns>
    public static PaginatedResponse<T> Success(
        T data,
        int totalRecords,
        int currentPage,
        int pageSize,
        string message = "Acción realizada exitosamente.",
        int statusCode = 200)
    {
        return new PaginatedResponse<T>
        {
            Data = data,
            IsSuccess = true,
            Message = message,
            StatusCode = statusCode,
            TotalRecords = totalRecords,
            CurrentPage = currentPage,
            PageSize = pageSize
        };
    }

    /// <summary>
    /// Constructor para respuestas paginadas fallidas.
    /// </summary>
    public static PaginatedResponse<T> Fail(
        string message,
        int statusCode = 400,
        int currentPage = 1,
        int pageSize = 10)
    {
        return new PaginatedResponse<T>
        {
            Data = default,
            IsSuccess = false,
            Message = message,
            StatusCode = statusCode,
            TotalRecords = 0,
            CurrentPage = currentPage,
            PageSize = pageSize
        };
    }
}
