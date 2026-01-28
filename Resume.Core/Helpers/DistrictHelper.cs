using Resume.Core.DTOs;
using Resume.Core.ExternalServiceContracts;

namespace Resume.Core.Helpers;

public static class DistrictHelper
{
    public static async Task<DistrictResponse?> MapDistrict(int? districtId, int? provinceId, ICompanyServiceClient companyService)
    {
        if (!districtId.HasValue || !provinceId.HasValue)
            return null;

        var response = await companyService.GetDistrictById(districtId.Value, provinceId.Value);
        if (!response.IsSuccess || response.Data == null)
            return null;

        return response.Data;
    }
}
