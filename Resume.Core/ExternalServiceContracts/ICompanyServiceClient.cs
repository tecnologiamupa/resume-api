using Resume.Core.DTOs;

namespace Resume.Core.ExternalServiceContracts;

public interface ICompanyServiceClient
{
    Task<BaseResponse<List<ProvinceResponse?>>> GetProvinces();
    Task<BaseResponse<ProvinceResponse?>> GetProvinceById(int id);
    Task<BaseResponse<List<DistrictResponse?>>> GetDistricts();
    Task<BaseResponse<DistrictResponse?>> GetDistrictById(int id, int provinceId);
    Task<BaseResponse<List<TownshipResponse?>>> GetTownships();
    Task<BaseResponse<TownshipResponse?>> GetTownshipById(int id, int provinceId, int districtId);
    /// <summary>
    /// Verifica si un currículum tiene un estado específico en alguna vacante de una empresa.
    /// </summary>
    /// <param name="companyId">Id de la empresa.</param>
    /// <param name="resumeId">Id del currículum.</param>
    /// <param name="statusId">Id del estado (enum VacancyResumeStatusEnum).</param>
    /// <returns>Respuesta con true si existe, false si no.</returns>
    Task<BaseResponse<bool>> ExistsResumeStatusAsync(Guid companyId, Guid resumeId, int statusId);
}
