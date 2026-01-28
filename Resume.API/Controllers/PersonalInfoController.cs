using Microsoft.AspNetCore.Mvc;
using Resume.Core.ServiceContracts;

namespace Resume.API.Controllers
{
    [Route("api/personal-info")]
    [ApiController]
    public class PersonalInfoController : ControllerBase
    {
        private readonly IPersonalInfoService _personalInfoService;
        public PersonalInfoController(IPersonalInfoService personalInfoService)
        {
            _personalInfoService = personalInfoService;
        }

        [HttpGet("identity-number/{identityNumber}")] // GET api/personal-info/identity-number/{identityNumber}
        public async Task<IActionResult> GetPersonalInfoByIdentityNumber(string identityNumber)
        {
            var response = await _personalInfoService.GetPersonalInfoByIdentityNumber(identityNumber);
            return StatusCode(response.StatusCode, response);
        }
    }
}
