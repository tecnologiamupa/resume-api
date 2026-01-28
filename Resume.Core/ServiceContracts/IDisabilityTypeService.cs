using Resume.Core.DTOs;

namespace Resume.Core.ServiceContracts;

public interface IDisabilityTypeService
{
    Task<BaseResponse<List<DisabilityTypeResponse?>>> GetDisabilityTypes();
    Task<BaseResponse<DisabilityTypeResponse?>> GetDisabilityTypeById(int id);
}
