using Resume.Core.DTOs;
using Resume.Core.ExternalServiceContracts;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;

namespace Resume.Infrastructure.ExternalServices;

public class CompanyServiceClient : ICompanyServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _memoryCache;

    public CompanyServiceClient(HttpClient httpClient, IMemoryCache memoryCache)
    {
        _httpClient = httpClient;
        _memoryCache = memoryCache;
    }

    public async Task<BaseResponse<List<DistrictResponse?>>> GetDistricts()
    {
        const string cacheKey = "districts_all";
        if (_memoryCache.TryGetValue(cacheKey, out BaseResponse<List<DistrictResponse?>> cachedDistricts))
            return cachedDistricts;

        try
        {
            var response = await _httpClient.GetAsync("/api/districts");
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return BaseResponse<List<DistrictResponse?>>.Fail(
                    $"Error al obtener los distritos",
                    (int)response.StatusCode
                );
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<BaseResponse<List<DistrictResponse?>>>();
            if (apiResponse is null)
            {
                return BaseResponse<List<DistrictResponse?>>.Fail(
                    "No se pudo deserializar la respuesta del servicio de compañía.",
                    (int)response.StatusCode
                );
            }

            _memoryCache.Set(cacheKey, apiResponse, TimeSpan.FromHours(12));
            return apiResponse;
        }
        catch (HttpRequestException)
        {
            return BaseResponse<List<DistrictResponse?>>.Fail(
                "Error de comunicación con el servicio de compañía"
            );
        }
    }

    public async Task<BaseResponse<DistrictResponse?>> GetDistrictById(int id, int provinceId)
    {
        var cacheKey = $"district_{provinceId}_{id}";
        if (_memoryCache.TryGetValue(cacheKey, out BaseResponse<DistrictResponse?> cachedDistrict))
            return cachedDistrict;

        try
        {
            var response = await _httpClient.GetAsync($"/api/districts/{provinceId}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return BaseResponse<DistrictResponse?>.Fail(
                    $"Error al obtener el distrito",
                    (int)response.StatusCode
                );
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<BaseResponse<DistrictResponse?>>();
            if (apiResponse is null)
            {
                return BaseResponse<DistrictResponse?>.Fail(
                    "No se pudo deserializar la respuesta del servicio de compañía.",
                    (int)response.StatusCode
                );
            }

            _memoryCache.Set(cacheKey, apiResponse, TimeSpan.FromHours(12));
            return apiResponse;
        }
        catch (HttpRequestException)
        {
            return BaseResponse<DistrictResponse?>.Fail(
                "Error de comunicación con el servicio de compañía"
            );
        }
    }

    public async Task<BaseResponse<List<ProvinceResponse?>>> GetProvinces()
    {
        const string cacheKey = "provinces_all";
        if (_memoryCache.TryGetValue(cacheKey, out BaseResponse<List<ProvinceResponse?>> cachedProvinces))
            return cachedProvinces;

        try
        {
            var response = await _httpClient.GetAsync("/api/provinces");
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return BaseResponse<List<ProvinceResponse?>>.Fail(
                    $"Error al obtener las provincias",
                    (int)response.StatusCode
                );
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<BaseResponse<List<ProvinceResponse?>>>();
            if (apiResponse is null)
            {
                return BaseResponse<List<ProvinceResponse?>>.Fail(
                    "No se pudo deserializar la respuesta del servicio de compañía.",
                    (int)response.StatusCode
                );
            }

            _memoryCache.Set(cacheKey, apiResponse, TimeSpan.FromHours(12));
            return apiResponse;
        }
        catch (HttpRequestException)
        {
            return BaseResponse<List<ProvinceResponse?>>.Fail(
                "Error de comunicación con el servicio de compañía"
            );
        }
    }

    public async Task<BaseResponse<ProvinceResponse?>> GetProvinceById(int id)
    {
        var cacheKey = $"province_{id}";
        if (_memoryCache.TryGetValue(cacheKey, out BaseResponse<ProvinceResponse?> cachedProvince))
            return cachedProvince;

        try
        {
            var response = await _httpClient.GetAsync($"/api/provinces/{id}");
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return BaseResponse<ProvinceResponse?>.Fail(
                    $"Error al obtener la provincia",
                    (int)response.StatusCode
                );
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<BaseResponse<ProvinceResponse?>>();
            if (apiResponse is null)
            {
                return BaseResponse<ProvinceResponse?>.Fail(
                    "No se pudo deserializar la respuesta del servicio de compañía.",
                    (int)response.StatusCode
                );
            }

            _memoryCache.Set(cacheKey, apiResponse, TimeSpan.FromHours(12));
            return apiResponse;
        }
        catch (HttpRequestException)
        {
            return BaseResponse<ProvinceResponse?>.Fail(
                "Error de comunicación con el servicio de compañía"
            );
        }
    }

    public async Task<BaseResponse<List<TownshipResponse?>>> GetTownships()
    {
        const string cacheKey = "townships_all";
        if (_memoryCache.TryGetValue(cacheKey, out BaseResponse<List<TownshipResponse?>> cachedTownships))
            return cachedTownships;

        try
        {
            var response = await _httpClient.GetAsync("/api/townships");
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return BaseResponse<List<TownshipResponse?>>.Fail(
                    $"Error al obtener los corregimientos",
                    (int)response.StatusCode
                );
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<BaseResponse<List<TownshipResponse?>>>();
            if (apiResponse is null)
            {
                return BaseResponse<List<TownshipResponse?>>.Fail(
                    "No se pudo deserializar la respuesta del servicio de compañía.",
                    (int)response.StatusCode
                );
            }

            _memoryCache.Set(cacheKey, apiResponse, TimeSpan.FromHours(12));
            return apiResponse;
        }
        catch (HttpRequestException)
        {
            return BaseResponse<List<TownshipResponse?>>.Fail(
                "Error de comunicación con el servicio de compañía"
            );
        }
    }

    public async Task<BaseResponse<TownshipResponse?>> GetTownshipById(int id, int districtId, int provinceId)
    {
        var cacheKey = $"township_{provinceId}_{districtId}_{id}";
        if (_memoryCache.TryGetValue(cacheKey, out BaseResponse<TownshipResponse?> cachedTownship))
            return cachedTownship;

        try
        {
            var response = await _httpClient.GetAsync($"/api/townships/{provinceId}/{districtId}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return BaseResponse<TownshipResponse?>.Fail(
                    $"Error al obtener el corregimiento",
                    (int)response.StatusCode
                );
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<BaseResponse<TownshipResponse?>>();
            if (apiResponse is null)
            {
                return BaseResponse<TownshipResponse?>.Fail(
                    "No se pudo deserializar la respuesta del servicio de compañía.",
                    (int)response.StatusCode
                );
            }

            _memoryCache.Set(cacheKey, apiResponse, TimeSpan.FromHours(12));
            return apiResponse;
        }
        catch (HttpRequestException)
        {
            return BaseResponse<TownshipResponse?>.Fail(
                "Error de comunicación con el servicio de compañía"
            );
        }
    }

    public async Task<BaseResponse<bool>> ExistsResumeStatusAsync(Guid companyId, Guid resumeId, int statusId)
    {
        var url = $"/api/vacancy-resume-status/exists-by-status?companyId={companyId}&resumeId={resumeId}&statusId={statusId}";
        try
        {
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return BaseResponse<bool>.Fail($"Error al verificar el estado del currículum: {errorMessage}", (int)response.StatusCode);
            }
            var apiResponse = await response.Content.ReadFromJsonAsync<BaseResponse<bool>>();
            if (apiResponse is null)
            {
                return BaseResponse<bool>.Fail("No se pudo deserializar la respuesta del servicio de empresa.", (int)response.StatusCode);
            }
            return apiResponse;
        }
        catch (HttpRequestException)
        {
            return BaseResponse<bool>.Fail("Error de comunicación con el servicio de empresa");
        }
    }
}