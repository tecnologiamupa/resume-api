namespace Resume.Core.DTOs.ResumeInfo
{
    /// <summary>
    /// Solicitud para obtener el currículum por identityNumber y mobile.
    /// </summary>
    public class ResumeLoginRequest
    {
        public string IdentityNumber { get; set; } = string.Empty;
        public string PhoneCountryCode { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public Guid? CompanyId { get; set; }
    }
}
