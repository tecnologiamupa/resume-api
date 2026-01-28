using Microsoft.AspNetCore.Http;

namespace Resume.Core.DTOs;

public class ProfileImageSaveRequest
{
    public Guid PersonalInfoId { get; set; }
    public IFormFile ProfileImage { get; set; }
}
