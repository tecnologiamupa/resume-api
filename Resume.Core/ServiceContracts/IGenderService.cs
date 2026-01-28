using Resume.Core.DTOs;

namespace Resume.Core.ServiceContracts;

public interface IGenderService
{
    Task<BaseResponse<List<GenderResponse?>>> GetGenders();
    Task<BaseResponse<GenderResponse?>> GetGenderById(int id);
}
