namespace Resume.Core.Helpers
{
    public static class PhoneHelper
    {
        /// <summary>
        /// Limpia el número de móvil eliminando todos los caracteres que no sean dígitos.
        /// </summary>
        public static string CleanMobile(string? mobile)
        {
            return string.IsNullOrWhiteSpace(mobile)
            ? string.Empty
            : new string(mobile.Where(char.IsDigit).ToArray());
        }
    }
}
