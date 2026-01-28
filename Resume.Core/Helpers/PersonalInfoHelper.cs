using Resume.Core.DTOs;

namespace Resume.Core.Helpers;

public static class PersonalInfoHelper
{
    /// <summary>
    /// Verifica si ProfilePhotoUrl tiene valor y lo concatena con la baseUrl.
    /// </summary>
    /// <param name="response">El objeto PersonalInfoResponse.</param>
    /// <param name="baseUrl">La URL base a concatenar.</param>
    /// <returns>La URL completa si ProfilePhotoUrl tiene valor; de lo contrario, null.</returns>
    public static string? GetFullProfilePhotoUrl(PersonalInfoResponse response, string baseUrl)
    {
        if (!string.IsNullOrEmpty(response.ProfilePhotoUrl))
        {
            return $"{baseUrl.TrimEnd('/')}/{response.ProfilePhotoUrl.TrimStart('/')}";
        }

        return null;
    }
}