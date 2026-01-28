using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Resume.Core.DTOs;
using Resume.Core.Entities;
using Resume.Core.ExternalServiceContracts;
using Resume.Core.Helpers;
using Resume.Core.RepositoryContracts;
using Resume.Core.ServiceContracts;

namespace Resume.Core.Services;

internal class PersonalInfoService : IPersonalInfoService
{
    private readonly IPersonalInfoRepository _personalInfoRepository;
    private readonly IMapper _mapper;
    private readonly IDisabilityTypeService _disabilityTypeService;
    private readonly IGenderService _genderService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICompanyServiceClient _companyServiceClient;
    private readonly IScheduleCounterRepository _scheduleCounterRepository;
    private readonly string _imageBaseUrl;

    public PersonalInfoService(
        IPersonalInfoRepository personalInfoRepository, 
        IMapper mapper,
        IConfiguration configuration,
        IDisabilityTypeService disabilityTypeService,
        IGenderService genderService,
        IHttpContextAccessor httpContextAccessor,
        ICompanyServiceClient companyServiceClient,
        IScheduleCounterRepository scheduleCounterRepository)
    {
        _personalInfoRepository = personalInfoRepository;
        _mapper = mapper;
        _imageBaseUrl = configuration["Files:BaseUrl"] ?? string.Empty;
        _disabilityTypeService = disabilityTypeService;
        _genderService = genderService;
        _httpContextAccessor = httpContextAccessor;
        _companyServiceClient = companyServiceClient;
        _scheduleCounterRepository = scheduleCounterRepository;

    }

    public async Task<BaseResponse<List<PersonalInfoResponse?>>> GetPersonalInfos()
    {
        var personalInfos = await _personalInfoRepository.GetPersonalInfos();

        // Usar el método MapPersonalInfosToResponses para mapear la colección
        var responses = await MapPersonalInfosToResponses(personalInfos);

        return BaseResponse<List<PersonalInfoResponse?>>.Success(responses);
    }

    public async Task<BaseResponse<PersonalInfoResponse?>> GetPersonalInfoById(Guid id)
    {
        var personalInfo = await _personalInfoRepository.GetPersonalInfoById(id);
        if (personalInfo == null)
            return BaseResponse<PersonalInfoResponse?>.Fail("Información personal no encontrada", 404);

        var response = _mapper.Map<PersonalInfoResponse>(personalInfo);

        response.Gender = await MapGender(personalInfo.GenderId);
        response.DisabilityType = await MapDisabilityType(personalInfo.DisabilityTypeId);
        response.Province = await ProvinceHelper.MapProvince(personalInfo.ProvinceId, _companyServiceClient);
        response.District = await DistrictHelper.MapDistrict(personalInfo.DistrictId, personalInfo.ProvinceId, _companyServiceClient);
        response.Township = await TownshipHelper.MapTownship(personalInfo.TownshipId, personalInfo.DistrictId, personalInfo.ProvinceId, _companyServiceClient);

        return BaseResponse<PersonalInfoResponse?>.Success(response);
    }

    public async Task<BaseResponse<PersonalInfoResponse?>> GetPersonalInfoByIdentityNumber(string identityNumber)
    {
        var personalInfo = await _personalInfoRepository.GetPersonalInfoByIdentityNumber(identityNumber);
        if (personalInfo == null)
            return BaseResponse<PersonalInfoResponse?>.Fail("Información personal no encontrada", 404);

        var response = _mapper.Map<PersonalInfoResponse>(personalInfo);

        return BaseResponse<PersonalInfoResponse?>.Success(response);
    }

    public async Task<BaseResponse<PersonalInfoResponse?>> GetPersonalInfoByMobile(string mobile)
    {
        var personalInfo = await _personalInfoRepository.GetPersonalInfoByMobile(mobile);
        if (personalInfo == null)
            return BaseResponse<PersonalInfoResponse?>.Fail("Información personal no encontrada", 404);

        var response = _mapper.Map<PersonalInfoResponse>(personalInfo);

        return BaseResponse<PersonalInfoResponse?>.Success(response);
    }

    public async Task<BaseResponse<PersonalInfoResponse>> CreatePersonalInfo(PersonalInfoCreateRequest personalInfoRequest)
    {
        var personalInfo = _mapper.Map<PersonalInfo>(personalInfoRequest);
        personalInfo.Id = Guid.NewGuid();
        personalInfo.CreatedDate = DateTimeHelper.GetCurrentDateTime();
        personalInfo.CreatedBy = UserContextHelper.GetCurrentUserId(_httpContextAccessor);
        personalInfo.ScheduleId = await ScheduleHelper.GetNextScheduleId(_scheduleCounterRepository);

        var created = await _personalInfoRepository.CreatePersonalInfo(personalInfo);
        if (created == null)
            return BaseResponse<PersonalInfoResponse>.Fail("No se pudo crear la información personal", 400);

        var response = _mapper.Map<PersonalInfoResponse>(created);
        return BaseResponse<PersonalInfoResponse>.Success(response, "Información personal creada exitosamente.", 201);
    }

    public async Task<BaseResponse<bool>> UpdatePersonalInfo(Guid id, PersonalInfoUpdateRequest personalInfoRequest)
    {
        var existing = await _personalInfoRepository.GetPersonalInfoById(id);
        if (existing == null)
            return BaseResponse<bool>.Fail("Información personal no encontrada", 404);

        // Actualiza los campos que no son nulos en la solicitud
        if (personalInfoRequest.FirstName != null) existing.FirstName = personalInfoRequest.FirstName;
        if (personalInfoRequest.LastName != null) existing.LastName = personalInfoRequest.LastName;
        if (personalInfoRequest.IdentityNumber != null) existing.IdentityNumber = personalInfoRequest.IdentityNumber;
        if (personalInfoRequest.BirthDate != null) existing.BirthDate = personalInfoRequest.BirthDate;
        if (personalInfoRequest.Title != null) existing.Title = personalInfoRequest.Title;
        if (personalInfoRequest.Email != null) existing.Email = personalInfoRequest.Email;
        if (personalInfoRequest.PhoneCountryCode != null) existing.PhoneCountryCode = personalInfoRequest.PhoneCountryCode;
        if (personalInfoRequest.Mobile != null) existing.Mobile = personalInfoRequest.Mobile;
        if (personalInfoRequest.City != null) existing.City = personalInfoRequest.City;
        if (personalInfoRequest.Country != null) existing.Country = personalInfoRequest.Country;
        if (personalInfoRequest.ProvinceId.HasValue) existing.ProvinceId = personalInfoRequest.ProvinceId.Value;
        if (personalInfoRequest.DistrictId.HasValue) existing.DistrictId = personalInfoRequest.DistrictId.Value;
        if (personalInfoRequest.TownshipId.HasValue) existing.TownshipId = personalInfoRequest.TownshipId.Value;
        if (personalInfoRequest.GenderId.HasValue) existing.GenderId = personalInfoRequest.GenderId.Value;
        if (personalInfoRequest.HasDisability.HasValue) existing.HasDisability = personalInfoRequest.HasDisability.Value;
        if (personalInfoRequest.DisabilityTypeId.HasValue) existing.DisabilityTypeId = personalInfoRequest.DisabilityTypeId.Value;
        if (personalInfoRequest.DisabilityDescription != null)
            existing.DisabilityDescription = personalInfoRequest.DisabilityDescription;

        existing.LastModifiedDate = DateTimeHelper.GetCurrentDateTime();
        existing.LastModifiedBy = UserContextHelper.GetCurrentUserId(_httpContextAccessor);

        var result = await _personalInfoRepository.UpdatePersonalInfo(existing);
        return result
            ? BaseResponse<bool>.Success(true)
            : BaseResponse<bool>.Fail("No se pudo actualizar la información personal", 400);
    }

    public async Task<BaseResponse<bool>> DeletePersonalInfo(Guid id)
    {
        var result = await _personalInfoRepository.DeletePersonalInfo(id);
        return result
            ? BaseResponse<bool>.Success(true)
            : BaseResponse<bool>.Fail("No se pudo eliminar la información personal", 400);
    }

    public async Task<BaseResponse<PersonalInfoResponse?>> GetPersonalInfoByIdentityNumberAndMobile(string identityNumber, string phoneCountryCode, string mobile)
    {
        var cleanedMobile = PhoneHelper.CleanMobile(mobile);
        var personalInfo = await _personalInfoRepository.GetPersonalInfoByIdentityNumberAndMobile(identityNumber, phoneCountryCode, cleanedMobile);
        if (personalInfo == null)
            return BaseResponse<PersonalInfoResponse?>.Fail("Información personal no encontrada",404);

        var response = _mapper.Map<PersonalInfoResponse>(personalInfo);
        return BaseResponse<PersonalInfoResponse?>.Success(response);
    }

    #region Métodos Auxiliares

    private async Task<GenderResponse?> MapGender(int? genderId)
    {
        if (genderId == null) return null;

        var response = await _genderService.GetGenderById(genderId.Value);
        if (!response.IsSuccess || response.Data == null)
        {
            return null;
        }

        return response.Data;
    }

    private async Task<DisabilityTypeResponse?> MapDisabilityType(int? disabilityTypeId)
    {
        if (disabilityTypeId == null) return null;

        var response = await _disabilityTypeService.GetDisabilityTypeById(disabilityTypeId.Value);
        if (!response.IsSuccess || response.Data == null)
        {
            return null;
        }

        return response.Data;
    }

    /// <summary>
    /// Mapea una colección de información personal a sus respuestas correspondientes.
    /// </summary>
    /// <param name="personalInfos">Colección de información personal.</param>
    /// <returns>Lista de respuestas de información personal.</returns>
    private async Task<List<PersonalInfoResponse?>> MapPersonalInfosToResponses(IEnumerable<PersonalInfo?> personalInfos)
    {
        var personalInfoResponses = new List<PersonalInfoResponse?>();

        foreach (var personalInfo in personalInfos)
        {
            if (personalInfo == null) continue;

            // Mapear la información personal base
            var response = _mapper.Map<PersonalInfoResponse?>(personalInfo);

            if (response != null)
            {
                // Mapear el género usando MapGender
                response.Gender = await MapGender(personalInfo.GenderId);

                // Mapear el tipo de discapacidad usando MapDisabilityType
                response.DisabilityType = await MapDisabilityType(personalInfo.DisabilityTypeId);

                personalInfoResponses.Add(response);
            }
        }

        return personalInfoResponses;
    }    

    #endregion
}