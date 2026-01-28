using Resume.Core.DTOs;
using Resume.Core.ExternalServiceContracts;

namespace Resume.Core.Helpers;

public static class TownshipHelper
{
    public static async Task<TownshipResponse?> MapTownship(int? townshipId, int? districtId, int? provinceId, ICompanyServiceClient companyService)
    {
        if (!townshipId.HasValue || !districtId.HasValue || !provinceId.HasValue)
            return null;

        var response = await companyService.GetTownshipById(townshipId.Value, districtId.Value, provinceId.Value);
        if (!response.IsSuccess || response.Data == null)
            return null;

        return response.Data;
    }
}
