using Resume.Core.DTOs;
using Resume.Core.ExternalServiceContracts;

namespace Resume.Core.Helpers;

public static class ProvinceHelper
{
    public static async Task<ProvinceResponse?> MapProvince(int? provinceId, ICompanyServiceClient companyService)
    {
        if (!provinceId.HasValue)
            return null;

        var response = await companyService.GetProvinceById(provinceId.Value);
        if (!response.IsSuccess || response.Data == null)
            return null;

        return response.Data;
    }
}
