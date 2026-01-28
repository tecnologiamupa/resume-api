namespace Resume.Core.DTOs;

/// <summary>
/// Clase base para las respuestas de la API.
/// </summary>
/// <typeparam name="T">Tipo de dato de la respuesta.</typeparam>
public class BaseResponse<T>
{
    public int StatusCode { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Constructor para respuestas exitosas.
    /// </summary>
    /// <param name="data">Datos de la respuesta.</param>
    /// <param name="message">Mensaje de la respuesta.</param>
    /// <param name="statusCode">Código de estado HTTP.</param>
    /// <returns>Una instancia de <see cref="BaseResponse{T}"/> con los datos de éxito.</returns>
    public static BaseResponse<T> Success(T data, string message = "Acción realizada exitosamente.", int statusCode = 200)
    {
        return new BaseResponse<T> { Data = data, IsSuccess = true, Message = message, StatusCode = statusCode };
    }

    /// <summary>
    /// Constructor para respuestas de error.
    /// </summary>
    /// <param name="message">Mensaje de error.</param>
    /// <param name="statusCode">Código de estado HTTP.</param>
    /// <returns>Una instancia de <see cref="BaseResponse{T}"/> con los datos de error.</returns>
    public static BaseResponse<T> Fail(string message, int statusCode = 400)
    {
        return new BaseResponse<T> { Data = default, IsSuccess = false, Message = message, StatusCode = statusCode };
    }

}
