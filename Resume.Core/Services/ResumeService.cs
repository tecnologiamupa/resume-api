using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Resume.Core.DTOs;
using Resume.Core.DTOs.ResumeInfo;
using Resume.Core.Entities;
using Resume.Core.Enums;
using Resume.Core.ExternalServiceContracts;
using Resume.Core.Helpers;
using Resume.Core.RepositoryContracts;
using Resume.Core.ServiceContracts;
using Sigueme.Core.DTOs;
using System.Security.Claims;

namespace Resume.Core.Services;

/// <summary>
/// Servicio para la gestión de currículums.
/// </summary>
internal class ResumeService : IResumeService
{
    private readonly IResumeRepository _resumeRepository;
    private readonly IPersonalInfoService _personalInfoService;
    private readonly IProfessionalResumeService _professionalResumeService;
    private readonly ISportsResumeService _sportsResumeService;
    private readonly IInternshipResumeService _internshipResumeService;
    private readonly IWorkExperienceService _workExperienceService;
    private readonly ISoftSkillService _softSkillService;
    private readonly ITechnicalSkillService _technicalSkillService;
    private readonly ILanguageService _languageService;
    private readonly IAcademicEducationService _academicEducationService;
    private readonly IProfessionalSkillService _professionalSkillService;
    private readonly IGenderService _genderService;
    private readonly ICompanyServiceClient _companyService;
    private readonly IDisabilityTypeService _disabilityTypeService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="ResumeService"/>.
    /// </summary>
    /// <param name="resumeRepository">Repositorio de currículums.</param>
    /// <param name="personalInfoService">Servicio de información personal.</param>
    /// <param name="professionalResumeService">Servicio de currículum profesional.</param>
    /// <param name="sportsResumeService">Servicio de currículum deportivo.</param>
    /// <param name="internshipResumeService">Servicio de currículum de pasantía.</param>
    /// <param name="mapper">Instancia de AutoMapper.</param>
    public ResumeService(
        IResumeRepository resumeRepository,
        IPersonalInfoService personalInfoService,
        IProfessionalResumeService professionalResumeService,
        ISportsResumeService sportsResumeService,
        IInternshipResumeService internshipResumeService,
        IMapper mapper,
        IWorkExperienceService workExperienceService,
        ISoftSkillService softSkillService,
        ITechnicalSkillService technicalSkillService,
        ILanguageService languageService,
        IAcademicEducationService academicEducationService,
        IHttpContextAccessor httpContextAccessor,
        IProfessionalSkillService professionalSkillService,
        IGenderService genderService,
        ICompanyServiceClient companyServiceClient,
        IDisabilityTypeService disabilityTypeService)
    {
        _resumeRepository = resumeRepository;
        _personalInfoService = personalInfoService;
        _professionalResumeService = professionalResumeService;
        _sportsResumeService = sportsResumeService;
        _internshipResumeService = internshipResumeService;
        _mapper = mapper;
        _workExperienceService = workExperienceService;
        _softSkillService = softSkillService;
        _technicalSkillService = technicalSkillService;
        _languageService = languageService;
        _academicEducationService = academicEducationService;
        _httpContextAccessor = httpContextAccessor;
        _professionalSkillService = professionalSkillService;
        _genderService = genderService;
        _companyService = companyServiceClient;
        _disabilityTypeService = disabilityTypeService;
    }

    /// <summary>
    /// Obtiene la lista de currículums.
    /// </summary>
    /// <returns>Respuesta con la lista de currículums.</returns>
    public async Task<BaseResponse<List<ResumeResponse?>>> GetResumes()
    {
        var resumes = await _resumeRepository.GetResumes();
        var responses = _mapper.Map<List<ResumeResponse?>>(resumes);
        return BaseResponse<List<ResumeResponse?>>.Success(responses);
    }

    public async Task<PaginatedResponse<List<ResumeResponse?>>> GetPagedResumes(int pageNumber, int pageSize)
    {
        // Normaliza la paginación
        PaginationHelper.NormalizePagination(ref pageNumber, ref pageSize);

        // Llama al repositorio para obtener los currículums completados y el total de registros
        var (resumes, totalRecords) = await _resumeRepository.GetPagedResumes(pageNumber, pageSize);

        // Mapea los currículums a respuestas
        var resumeResponses = await MapResumesToResponses(resumes);

        return PaginatedResponse<List<ResumeResponse?>>.Success(
            resumeResponses,
            totalRecords,
            pageNumber,
            pageSize
        );
    }

    public async Task<PaginatedResponse<List<ResumeResponse?>>> GetPagedResumesByFilter(ResumeFilterRequest filter, int pageNumber, int pageSize)
    {
        // Normaliza la paginación
        PaginationHelper.NormalizePagination(ref pageNumber, ref pageSize);

        // Llama al repositorio para obtener los currículums completados y el total de registros
        var (resumes, totalRecords) = await _resumeRepository.GetPagedResumesByFilter(filter, pageNumber, pageSize);

        // Mapea los currículums a respuestas
        var resumeResponses = await MapResumesToResponses(resumes);

        return PaginatedResponse<List<ResumeResponse?>>.Success(
            resumeResponses,
            totalRecords,
            pageNumber,
            pageSize
        );
    }

    public async Task<PaginatedResponse<List<ResumeResponse?>>> GetCompletedResumesPaged(int pageNumber, int pageSize)
    {
        // Normaliza la paginación
        PaginationHelper.NormalizePagination(ref pageNumber, ref pageSize);

        // Llama al repositorio para obtener los currículums completados y el total de registros
        var (resumes, totalRecords) = await _resumeRepository.GetCompletedResumesPaged(pageNumber, pageSize);

        // Mapea los currículums a respuestas
        var resumeResponses = await MapResumesToResponses(resumes);

        return PaginatedResponse<List<ResumeResponse?>>.Success(
            resumeResponses,
            totalRecords,
            pageNumber,
            pageSize
        );
    }

    public async Task<PaginatedResponse<List<ResumeResponse?>>> GetIncompleteResumesPaged(int pageNumber, int pageSize)
    {
        // Normaliza la paginación
        PaginationHelper.NormalizePagination(ref pageNumber, ref pageSize);

        // Llama al repositorio para obtener los currículums incompletos y el total de registros
        var (resumes, totalRecords) = await _resumeRepository.GetIncompleteResumesPaged(pageNumber, pageSize);

        // Mapea los currículums a respuestas
        var resumeResponses = await MapResumesToResponses(resumes);

        return PaginatedResponse<List<ResumeResponse?>>.Success(
            resumeResponses,
            totalRecords,
            pageNumber,
            pageSize
        );
    }

    /// <summary>
    /// Obtiene la lista de currículums favoritos de una empresa, permitiendo duplicados por vacante y mostrando el nombre de la vacante.
    /// </summary>
    /// <param name="companyId">Identificador de la empresa.</param>
    /// <returns>Respuesta con la lista de currículums y el nombre de la vacante.</returns>
    public async Task<BaseResponse<List<ResumeWithVacancyResponse?>>> GetResumesByCompany(Guid companyId)
    {
        // Paso 1: Obtén todos los resumes con PersonalInfo embebido
        var resumes = (await _resumeRepository.GetResumesByCompany(companyId)).ToList();

        // Paso 2: Trae todos los catálogos de una sola vez
        var genders = await _genderService.GetGenders();
        var provinces = await _companyService.GetProvinces();
        var districts = await _companyService.GetDistricts();
        var townships = await _companyService.GetTownships();
        var disabilityTypes = await _disabilityTypeService.GetDisabilityTypes();

        // Paso 3: Crea diccionarios para acceso rápido (usando claves compuestas donde corresponde)
        var genderDict = genders.Data!.ToDictionary(g => g.Id);
        var provinceDict = provinces.Data!.ToDictionary(p => p.Id);
        var districtDict = districts.Data!.ToDictionary(d => (d.ProvinceId, d.Id));
        var townshipDict = townships.Data!.ToDictionary(t => (t.ProvinceId, t.DistrictId, t.Id));
        var disabilityTypeDict = disabilityTypes.Data!.ToDictionary(dt => dt.Id);        

        // Paso 4: Asigna los catálogos
        foreach (var resume in resumes)
        {
            var pi = resume.PersonalInfo;
            if (pi == null) continue;

            if (pi.GenderId.HasValue && genderDict.TryGetValue(pi.GenderId.Value, out var gender))
                pi.Gender = gender;

            if (pi.ProvinceId.HasValue && provinceDict.TryGetValue(pi.ProvinceId.Value, out var province))
                pi.Province = province;

            if (pi.DistrictId.HasValue && pi.ProvinceId.HasValue &&
                districtDict.TryGetValue((pi.ProvinceId.Value, pi.DistrictId.Value), out var district))
                pi.District = district;

            if (pi.TownshipId.HasValue && pi.ProvinceId.HasValue && pi.DistrictId.HasValue &&
                townshipDict.TryGetValue((pi.ProvinceId.Value, pi.DistrictId.Value, pi.TownshipId.Value), out var township))
                pi.Township = township;

            if (pi.DisabilityTypeId.HasValue && disabilityTypeDict.TryGetValue(pi.DisabilityTypeId.Value, out var disabilityType))
                pi.DisabilityType = disabilityType;
        }

        // Paso 5: Mapea a la respuesta final
        var responses = _mapper.Map<List<ResumeWithVacancyResponse?>>(resumes);

        return BaseResponse<List<ResumeWithVacancyResponse?>>.Success(responses);
    }

    public async Task<BaseResponse<ResumeDetailResponse?>> GetMyResume()
    {
        // Obtener el ID del usuario actual desde el contexto HTTP
        var userId = UserContextHelper.GetCurrentUserId(_httpContextAccessor);
        if (string.IsNullOrEmpty(userId))
        {
            return BaseResponse<ResumeDetailResponse?>.Fail("No autorizado.", 401);
        }

        // Convertir el ID del usuario a Guid
        if (!Guid.TryParse(userId, out var personalInfoId))
        {
            return BaseResponse<ResumeDetailResponse?>.Fail("No autorizado.", 401);
        }

        var resume = await _resumeRepository.GetResumeByPersonalInfoId(personalInfoId);
        if (resume == null)
            return BaseResponse<ResumeDetailResponse?>.Fail("Curriculum no encontrado", 404);

        var response = _mapper.Map<ResumeDetailResponse>(resume);

        // Mapea la información personal si existe
        response.PersonalInfo = await MapPersonalInfo(resume.PersonalInfoId);

        // Mapea los idiomas si existe información personal
        response.Languages = await MapLanguages(resume.PersonalInfoId);

        // Verifica el tipo de currículum
        if (resume.ResumeTypeId == (int)ResumeTypeEnum.Professional)
        {
            // Obtener el ProfessionalResume asociado al ResumeId
            var professionalResumeResponse = await _professionalResumeService.GetProfessionalResumeByResumeId(resume.Id);
            if (professionalResumeResponse.IsSuccess && professionalResumeResponse.Data != null)
            {
                // Inicializa ProfessionalInfo con las WorkExperiences directamente
                response.ProfessionalInfo = new ProfessionalInfoResponse
                {
                    WorkExperiences = await MapWorkExperiences(professionalResumeResponse.Data.Id),
                    SoftSkills = await MapSoftSkills(professionalResumeResponse.Data.Id),
                    TechnicalSkills = await MapTechnicalSkills(professionalResumeResponse.Data.Id),
                    Skills = await MapSkills(professionalResumeResponse.Data.Id),
                    AcademicEducations = await MapAcademicEducations(professionalResumeResponse.Data.Id),
                    Internship = professionalResumeResponse.Data.Internship,
                    Inadeh = professionalResumeResponse.Data.Inadeh,
                    Platzi = professionalResumeResponse.Data.Platzi
                };
            }
        }

        return BaseResponse<ResumeDetailResponse?>.Success(response);
    }

    /// <summary>
    /// Obtiene el detalle de un currículum por su identificador.
    /// </summary>
    /// <param name="id">Identificador del currículum.</param>
    /// <returns>Respuesta con el detalle del currículum.</returns>
    public async Task<BaseResponse<ResumeDetailResponse?>> GetResumeById(Guid id)
    {
        var resume = await _resumeRepository.GetResumeById(id);
        if (resume == null)
            return BaseResponse<ResumeDetailResponse?>.Fail("Curriculum no encontrado", 404);

        var response = _mapper.Map<ResumeDetailResponse>(resume);

        // Mapea la información personal si existe
        response.PersonalInfo = await MapPersonalInfo(resume.PersonalInfoId);

        // Mapea los idiomas si existe información personal
        response.Languages = await MapLanguages(resume.PersonalInfoId);

        // Verifica el tipo de currículum
        if (resume.ResumeTypeId == (int)ResumeTypeEnum.Professional)
        {
            // Obtener el ProfessionalResume asociado al ResumeId
            var professionalResumeResponse = await _professionalResumeService.GetProfessionalResumeByResumeId(id);
            if (professionalResumeResponse.IsSuccess && professionalResumeResponse.Data != null)
            {
                // Inicializa ProfessionalInfo con las WorkExperiences directamente
                response.ProfessionalInfo = new ProfessionalInfoResponse
                {
                    WorkExperiences = await MapWorkExperiences(professionalResumeResponse.Data.Id),
                    SoftSkills = await MapSoftSkills(professionalResumeResponse.Data.Id),
                    TechnicalSkills = await MapTechnicalSkills(professionalResumeResponse.Data.Id),
                    Skills = await MapSkills(professionalResumeResponse.Data.Id),
                    AcademicEducations = await MapAcademicEducations(professionalResumeResponse.Data.Id),
                    Internship = professionalResumeResponse.Data.Internship,
                    Inadeh = professionalResumeResponse.Data.Inadeh,
                    Platzi = professionalResumeResponse.Data.Platzi
                };
            }
        }

        return BaseResponse<ResumeDetailResponse?>.Success(response);
    }

    /// <summary>
    /// Crea un nuevo currículum.
    /// </summary>
    /// <param name="resumeRequest">Datos para crear el currículum.</param>
    /// <returns>Respuesta con el currículum creado.</returns>
    public async Task<BaseResponse<ResumeResponse>> CreateResume(ResumeCreateRequest resumeRequest)
    {
        // Mapea la solicitud a la entidad
        var resume = _mapper.Map<ResumeInfo>(resumeRequest);
        resume.Id = Guid.NewGuid();
        resume.CreatedDate = DateTimeHelper.GetCurrentDateTime();
        resume.CreatedBy = UserContextHelper.GetCurrentUserId(_httpContextAccessor);

        // Intenta crear el currículum en el repositorio
        var resumeCreate = await _resumeRepository.CreateResume(resume);
        if (resumeCreate == null)
            return BaseResponse<ResumeResponse>.Fail("No se pudo crear el curriculum", 400);

        // Determina el tipo de currículum usando el enum
        var resumeType = (ResumeTypeEnum)resumeRequest.ResumeTypeId;

        // Según el tipo, crea la entidad específica asociada
        switch (resumeType)
        {
            case ResumeTypeEnum.Professional:
                var professionalCreateRequest = new ProfessionalResumeCreateRequest
                {
                    ResumeId = resume.Id,
                };
                // Crea el currículum profesional
                var professionalResult = await _professionalResumeService.CreateProfessionalResume(professionalCreateRequest);
                if (!professionalResult.IsSuccess)
                    return BaseResponse<ResumeResponse>.Fail("No se pudo crear el currículum profesional", 400);
                break;

            case ResumeTypeEnum.Sports:
                var sportsCreateRequest = new SportsResumeCreateRequest
                {
                    ResumeId = resume.Id
                };
                // Crea el currículum deportivo
                var sportsResult = await _sportsResumeService.CreateSportsResume(sportsCreateRequest);
                if (!sportsResult.IsSuccess)
                    return BaseResponse<ResumeResponse>.Fail("No se pudo crear el currículum deportivo", 400);
                break;

            case ResumeTypeEnum.Internship:
                var internshipCreateRequest = new InternshipResumeCreateRequest
                {
                    ResumeId = resume.Id
                };
                // Crea el currículum de pasantía
                var internshipResult = await _internshipResumeService.CreateInternshipResume(internshipCreateRequest);
                if (!internshipResult.IsSuccess)
                    return BaseResponse<ResumeResponse>.Fail("No se pudo crear el currículum de pasantía", 400);
                break;

            default:
                // Tipo de currículum no soportado
                return BaseResponse<ResumeResponse>.Fail("Tipo de currículum no soportado", 400);
        }

        // Mapea la entidad creada a la respuesta y retorna éxito
        var response = _mapper.Map<ResumeResponse>(resumeCreate);
        return BaseResponse<ResumeResponse>.Success(response, "Currículum creado exitosamente.", 201);
    }

    /// <summary>
    /// Actualiza un currículum existente.
    /// </summary>
    /// <param name="id">Identificador del currículum.</param>
    /// <param name="resumeRequest">Datos para actualizar el currículum.</param>
    /// <returns>Respuesta indicando si la actualización fue exitosa.</returns>
    public async Task<BaseResponse<ResumeResponse>> UpdateResume(Guid id, ResumeUpdateRequest resumeRequest)
    {
        var existingResume = await _resumeRepository.GetResumeById(id);
        if (existingResume == null)
            return BaseResponse<ResumeResponse>.Fail("Curriculum no encontrado", 404);

        if (existingResume.PersonalInfoId.HasValue)
        {
            // Obtener el token del encabezado de la solicitud
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                return BaseResponse<ResumeResponse>.Fail("No autorizado.", 401);
            }

            // Validar el token usando el helper
            var jwtSettings = _httpContextAccessor.HttpContext?.RequestServices.GetService<IConfiguration>()?.GetSection("JwtSettings");
            var secretKey = jwtSettings?["Key"];
            var validIssuer = jwtSettings?["Issuer"];
            var validAudience = jwtSettings?["Audience"];

            var principal = TokenValidatorHelper.ValidateToken(token, secretKey, validIssuer, validAudience);
            if (principal == null)
            {
                return BaseResponse<ResumeResponse>.Fail("No autorizado.", 401);
            }

            // Verificar si el usuario tiene el claim de identificación
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return BaseResponse<ResumeResponse>.Fail("No autorizado.", 401);
            }

            // Convertir el claim a Guid y comparar con PersonalInfoId
            if (!Guid.TryParse(userIdClaim, out var userId) || existingResume.PersonalInfoId != userId)
            {
                return BaseResponse<ResumeResponse>.Fail("No autorizado.", 401);
            }
        }

        // Actualizar campos básicos
        UpdateBasicFields(existingResume, resumeRequest);

        // Actualizar información personal
        var personalInfoResult = await UpdatePersonalInfo(existingResume, resumeRequest.PersonalInfo);
        if (!personalInfoResult.IsSuccess)
            return BaseResponse<ResumeResponse>.Fail(personalInfoResult.Message, personalInfoResult.StatusCode);

        // Actualizar idiomas
        var languagesResult = await ReplaceLanguages(existingResume.PersonalInfoId, resumeRequest.Languages);
        if (!languagesResult.IsSuccess)
            //return languagesResult;
            return BaseResponse<ResumeResponse>.Fail(languagesResult.Message, languagesResult.StatusCode);

        // Actualizar información profesional
        var professionalInfoResult = await UpdateProfessionalInfo(id, resumeRequest.ProfessionalInfo);
        if (!professionalInfoResult.IsSuccess)
            //return professionalInfoResult;
            return BaseResponse<ResumeResponse>.Fail(professionalInfoResult.Message, professionalInfoResult.StatusCode);

        existingResume.LastModifiedDate = DateTimeHelper.GetCurrentDateTime();
        existingResume.LastModifiedBy = UserContextHelper.GetCurrentUserId(_httpContextAccessor);

        var result = await _resumeRepository.UpdateResume(existingResume);

        // Mapea la entidad a la respuesta y retorna éxito
        var response = _mapper.Map<ResumeResponse>(existingResume);

        // Mapear la información personal
        response.PersonalInfo = await MapPersonalInfo(existingResume.PersonalInfoId);

        return result
            ? BaseResponse<ResumeResponse>.Success(response)
            : BaseResponse<ResumeResponse>.Fail("No se pudo actualizar el curriculum", 400);
    }

    /// <summary>
    /// Elimina un currículum por su identificador.
    /// </summary>
    /// <param name="id">Identificador del currículum.</param>
    /// <returns>Respuesta indicando si la eliminación fue exitosa.</returns>
    public async Task<BaseResponse<bool>> DeleteResume(Guid id)
    {
        var result = await _resumeRepository.DeleteResume(id);
        return result
            ? BaseResponse<bool>.Success(true)
            : BaseResponse<bool>.Fail("No se pudo eliminar el curriculum", 400);
    }

    #region Métodos Auxiliares

    /// <summary>
    /// Mapea un PersonalInfoResponse a partir de un PersonalInfoId.
    /// </summary>
    /// <param name="personalInfoId">Identificador de la información personal.</param>
    /// <returns>Instancia de PersonalInfoResponse o null si no existe.</returns>
    private async Task<PersonalInfoResponse?> MapPersonalInfo(Guid? personalInfoId)
    {
        if (!personalInfoId.HasValue || personalInfoId.Value == Guid.Empty)
            return null;

        var response = await _personalInfoService.GetPersonalInfoById(personalInfoId.Value);
        if (!response.IsSuccess || response.Data == null)
            return null;

        return response.Data;
    }

    /// <summary>
    /// Mapea las experiencias laborales asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <returns>Lista de WorkExperienceResponse o null si no existen.</returns>
    private async Task<List<WorkExperienceResponse>?> MapWorkExperiences(Guid professionalResumeId)
    {
        // Obtener las experiencias laborales asociadas al ProfessionalResumeId
        var workExperiencesResponse = await _workExperienceService.GetWorkExperiencesByProfessionalResumeId(professionalResumeId);
        if (!workExperiencesResponse.IsSuccess || workExperiencesResponse.Data == null)
            return null;

        return workExperiencesResponse.Data;
    }

    /// <summary>
    /// Mapea las habilidades blandas asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <returns>Lista de habilidades blandas como cadenas.</returns>
    private async Task<List<string>> MapSoftSkills(Guid professionalResumeId)
    {
        var softSkillsResponse = await _softSkillService.GetSoftSkillsByProfessionalResumeId(professionalResumeId);
        if (!softSkillsResponse.IsSuccess || softSkillsResponse.Data == null)
            return new List<string>();

        return softSkillsResponse.Data
            .Select(skill => skill.Skill ?? string.Empty)
            .Where(skill => !string.IsNullOrWhiteSpace(skill))
            .ToList();
    }

    /// <summary>
    /// Mapea las habilidades técnicas asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <returns>Lista de habilidades técnicas como cadenas.</returns>
    private async Task<List<string>> MapTechnicalSkills(Guid professionalResumeId)
    {
        var technicalSkillsResponse = await _technicalSkillService.GetTechnicalSkillsByProfessionalResumeId(professionalResumeId);
        if (!technicalSkillsResponse.IsSuccess || technicalSkillsResponse.Data == null)
            return new List<string>();

        return technicalSkillsResponse.Data
            .Select(skill => skill.Skill ?? string.Empty)
            .Where(skill => !string.IsNullOrWhiteSpace(skill))
            .ToList();
    }

    /// <summary>
    /// Mapea las habilidades profesionales asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <returns>Lista de ProfessionalSkillResponse o null si no existen.</returns>
    private async Task<List<ProfessionalSkillResponse>?> MapSkills(Guid professionalResumeId)
    {
        var skillsResponse = await _professionalSkillService.GetSkillsByProfessionalResumeId(professionalResumeId);
        if (!skillsResponse.IsSuccess || skillsResponse.Data == null)
            return null;

        return skillsResponse.Data;
    }

    /// <summary>
    /// Mapea las entradas de educación académica asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <returns>Lista de AcademicEducationResponse o null si no existen.</returns>
    private async Task<List<AcademicEducationResponse>?> MapAcademicEducations(Guid professionalResumeId)
    {
        var academicEducationResponse = await _academicEducationService.GetAcademicEducationsByProfessionalResumeId(professionalResumeId);
        if (!academicEducationResponse.IsSuccess || academicEducationResponse.Data == null)
            return null;

        return academicEducationResponse.Data;
    }

    /// <summary>
    /// Mapea los idiomas asociados a una información personal.
    /// </summary>
    /// <param name="personalInfoId">Identificador de la información personal.</param>
    /// <returns>Lista de LanguageResponse o null si no existen.</returns>
    private async Task<List<LanguageResponse>?> MapLanguages(Guid? personalInfoId)
    {
        if (!personalInfoId.HasValue || personalInfoId.Value == Guid.Empty)
            return null;

        var languagesResponse = await _languageService.GetLanguagesByPersonalInfoId(personalInfoId.Value);
        if (!languagesResponse.IsSuccess || languagesResponse.Data == null)
            return null;

        return languagesResponse.Data;
    }

    private void UpdateBasicFields(ResumeInfo existingResume, ResumeUpdateRequest resumeRequest)
    {
        if (resumeRequest.LinkedIn != null) existingResume.LinkedIn = resumeRequest.LinkedIn;
        if (resumeRequest.PortfolioUrl != null) existingResume.PortfolioUrl = resumeRequest.PortfolioUrl;
        if (resumeRequest.Summary != null) existingResume.Summary = resumeRequest.Summary;
    }

    private async Task<BaseResponse<PersonalInfoUpdateEnum>> UpdatePersonalInfo(ResumeInfo existingResume, PersonalInfoCreateRequest? personalInfoRequest)
    {
        if (personalInfoRequest == null || string.IsNullOrWhiteSpace(personalInfoRequest.IdentityNumber))
            return BaseResponse<PersonalInfoUpdateEnum>.Success(PersonalInfoUpdateEnum.None);

        Guid? personalInfoId = existingResume.PersonalInfoId;

        if (personalInfoId.HasValue)
        {
            var updateResult = await _personalInfoService.UpdatePersonalInfo(
                personalInfoId.Value,
                _mapper.Map<PersonalInfoUpdateRequest>(personalInfoRequest)
            );
            if (!updateResult.IsSuccess)
                return BaseResponse<PersonalInfoUpdateEnum>.Fail("No se pudo actualizar la información personal", 400);

            return BaseResponse<PersonalInfoUpdateEnum>.Success(PersonalInfoUpdateEnum.Updated);
        }
        else
        {
            var searchByMobileResult = await _personalInfoService.GetPersonalInfoByMobile(personalInfoRequest.Mobile);
            var personalInfoByMobile = searchByMobileResult.Data;

            if (personalInfoByMobile != null)
            {
                return BaseResponse<PersonalInfoUpdateEnum>.Fail("Ya existe una hoja de vida con este número de celular.", 400);
            }

            var searchResult = await _personalInfoService.GetPersonalInfoByIdentityNumber(personalInfoRequest.IdentityNumber);
            var personalInfo = searchResult.Data;

            if (personalInfo == null)
            {
                var createResult = await _personalInfoService.CreatePersonalInfo(personalInfoRequest);
                if (!createResult.IsSuccess || createResult.Data == null)
                    return BaseResponse<PersonalInfoUpdateEnum>.Fail("No se pudo crear la información personal", 400);

                existingResume.PersonalInfoId = createResult.Data.Id;
                return BaseResponse<PersonalInfoUpdateEnum>.Success(PersonalInfoUpdateEnum.Created);
            }
            else
            {
                return BaseResponse<PersonalInfoUpdateEnum>.Fail("Ya existe una hoja de vida con esta cédula.", 400);
            }
        }
    }

    private async Task<BaseResponse<bool>> UpdateProfessionalInfo(Guid resumeId, ProfessionalInfoRequest? professionalInfoRequest)
    {
        if (professionalInfoRequest == null)
            return BaseResponse<bool>.Success(true);

        var professionalResumeResponse = await _professionalResumeService.GetProfessionalResumeByResumeId(resumeId);
        if (!professionalResumeResponse.IsSuccess || professionalResumeResponse.Data == null)
            return BaseResponse<bool>.Fail("No se pudo obtener el ProfessionalResume asociado al currículum", 400);

        var professionalResumeId = professionalResumeResponse.Data.Id;

        // Actualizar el ProfessionalResume
        var updateProfessionalResumeResult = await UpdateProfessionalResume(professionalResumeResponse.Data, professionalInfoRequest);
        if (!updateProfessionalResumeResult.IsSuccess)
            return updateProfessionalResumeResult;

        // Actualizar experiencias laborales
        var workExperienceResult = await ReplaceWorkExperiences(professionalResumeId, professionalInfoRequest.WorkExperiences);
        if (!workExperienceResult.IsSuccess)
            return workExperienceResult;

        // Actualizar habilidades blandas
        var softSkillsResult = await ReplaceSoftSkills(professionalResumeId, professionalInfoRequest.SoftSkills);
        if (!softSkillsResult.IsSuccess)
            return softSkillsResult;

        // Actualizar habilidades técnicas
        var technicalSkillsResult = await ReplaceTechnicalSkills(professionalResumeId, professionalInfoRequest.TechnicalSkills);
        if (!technicalSkillsResult.IsSuccess)
            return technicalSkillsResult;

        // Actualizar habilidades profesionales
        var professionalSkillsResult = await ReplaceSkills(professionalResumeId, professionalInfoRequest.Skills);
        if (!professionalSkillsResult.IsSuccess)
            return professionalSkillsResult;

        // Actualizar educación académica
        var academicEducationResult = await ReplaceAcademicEducations(professionalResumeId, professionalInfoRequest.AcademicEducations);
        if (!academicEducationResult.IsSuccess)
            return academicEducationResult;

        return BaseResponse<bool>.Success(true);
    }

    /// <summary>
    /// Actualiza los idiomas asociados a la información personal.
    /// </summary>
    /// <param name="personalInfoId">Identificador de la información personal.</param>
    /// <param name="languages">Lista de idiomas a actualizar.</param>
    /// <returns>Respuesta indicando si la actualización fue exitosa.</returns>
    //private async Task<BaseResponse<bool>> UpdateLanguages(Guid? personalInfoId, List<string>? languages)
    //{
    //    // Validar que el PersonalInfoId sea válido
    //    if (personalInfoId == null || personalInfoId == Guid.Empty)
    //        return BaseResponse<bool>.Success(true);

    //    // Validar que la lista de idiomas no sea nula
    //    if (languages == null || !languages.Any())
    //        return BaseResponse<bool>.Success(true);

    //    // Mapear cada cadena a un LanguageCreateRequest
    //    var languageCreateRequests = languages
    //        .Where(language => !string.IsNullOrWhiteSpace(language)) // Filtrar valores nulos o vacíos
    //        .Select(language => new LanguageCreateRequest
    //        {
    //            PersonalInfoId = personalInfoId.Value,
    //            Name = language
    //        })
    //        .ToList();

    //    // Llamar al servicio para reemplazar los idiomas
    //    var replaceResult = await _languageService.ReplaceLanguages(personalInfoId.Value, languageCreateRequests);
    //    return replaceResult.IsSuccess
    //        ? BaseResponse<bool>.Success(true)
    //        : BaseResponse<bool>.Fail("No se pudieron actualizar los idiomas.", 400);
    //}

    /// <summary>
    /// Reemplaza los idiomas asociados a la información personal.
    /// </summary>
    /// <param name="personalInfoId">Identificador de la información personal.</param>
    /// <param name="languages">Lista de idiomas a reemplazar.</param>
    /// <returns>Respuesta indicando si la operación fue exitosa.</returns>
    private async Task<BaseResponse<bool>> ReplaceLanguages(Guid? personalInfoId, List<LanguageCreateRequest>? languages)
    {
        if (personalInfoId == null || personalInfoId == Guid.Empty)
            return BaseResponse<bool>.Success(true);

        if (languages == null || !languages.Any())
            return BaseResponse<bool>.Success(true);

        var replaceResult = await _languageService.ReplaceLanguages(personalInfoId.Value, languages);

        return replaceResult.IsSuccess
            ? BaseResponse<bool>.Success(true)
            : BaseResponse<bool>.Fail("No se pudieron reemplazar los idiomas.", 400);
    }

    private async Task<BaseResponse<bool>> ReplaceWorkExperiences(Guid professionalResumeId, List<WorkExperienceRequest>? workExperiences)
    {
        if (workExperiences == null)
            return BaseResponse<bool>.Success(true);

        // Realizar el mapeo de WorkExperienceRequest a WorkExperienceCreateRequest
        var workExperienceEntities = _mapper.Map<List<WorkExperienceCreateRequest>>(workExperiences);

        var replaceResult = await _workExperienceService.ReplaceWorkExperiences(professionalResumeId, workExperienceEntities);

        return replaceResult.IsSuccess
            ? BaseResponse<bool>.Success(true)
            : BaseResponse<bool>.Fail("No se pudieron reemplazar las experiencias laborales", 400);
    }

    private async Task<BaseResponse<bool>> ReplaceSoftSkills(Guid professionalResumeId, List<string>? softSkills)
    {
        if (softSkills == null || !softSkills.Any())
            return BaseResponse<bool>.Success(true);

        // Mapear cada cadena a un SoftSkillCreateRequest
        var softSkillEntities = softSkills
            .Where(skill => !string.IsNullOrWhiteSpace(skill)) // Filtrar valores nulos o vacíos
            .Select(skill => new SoftSkillCreateRequest
            {
                ProfessionalResumeId = professionalResumeId,
                Skill = skill
            })
            .ToList();

        var replaceResult = await _softSkillService.ReplaceSoftSkills(professionalResumeId, softSkillEntities);

        return replaceResult.IsSuccess
            ? BaseResponse<bool>.Success(true)
            : BaseResponse<bool>.Fail("No se pudieron reemplazar las habilidades blandas", 400);
    }

    private async Task<BaseResponse<bool>> ReplaceTechnicalSkills(Guid professionalResumeId, List<string>? technicalSkills)
    {
        if (technicalSkills == null || !technicalSkills.Any())
            return BaseResponse<bool>.Success(true);

        // Mapear cada cadena a un TechnicalSkillCreateRequest
        var technicalSkillEntities = technicalSkills
            .Where(skill => !string.IsNullOrWhiteSpace(skill)) // Filtrar valores nulos o vacíos
            .Select(skill => new TechnicalSkillCreateRequest
            {
                ProfessionalResumeId = professionalResumeId,
                Skill = skill
            })
            .ToList();

        var replaceResult = await _technicalSkillService.ReplaceTechnicalSkills(professionalResumeId, technicalSkillEntities);

        return replaceResult.IsSuccess
            ? BaseResponse<bool>.Success(true)
            : BaseResponse<bool>.Fail("No se pudieron reemplazar las habilidades técnicas", 400);
    }

    /// <summary>
    /// Reemplaza las habilidades profesionales asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <param name="skills">Lista de solicitudes de creación de habilidades profesionales.</param>
    /// <returns>Respuesta indicando si la operación fue exitosa.</returns>
    private async Task<BaseResponse<bool>> ReplaceSkills(Guid professionalResumeId, List<ProfessionalSkillCreateRequest>? skills)
    {
        if (skills == null || !skills.Any())
            return BaseResponse<bool>.Success(true);

        var replaceResult = await _professionalSkillService.ReplaceProfessionalSkills(professionalResumeId, skills);

        return replaceResult.IsSuccess
            ? BaseResponse<bool>.Success(true)
            : BaseResponse<bool>.Fail("No se pudieron reemplazar las habilidades profesionales.", 400);
    }

    /// <summary>
    /// Reemplaza todas las entradas de educación académica asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <param name="academicEducations">Lista de solicitudes de educación académica.</param>
    /// <returns>Respuesta indicando si la operación fue exitosa.</returns>
    private async Task<BaseResponse<bool>> ReplaceAcademicEducations(Guid professionalResumeId, List<AcademicEducationRequest>? academicEducations)
    {
        if (academicEducations == null || !academicEducations.Any())
            return BaseResponse<bool>.Success(true);

        // Mapear las solicitudes a entidades
        var academicEducationEntities = _mapper.Map<List<AcademicEducationCreateRequest>>(academicEducations);
        foreach (var academicEducation in academicEducationEntities)
        {
            academicEducation.ProfessionalResumeId = professionalResumeId;
        }

        // Llamar al servicio para reemplazar las entradas de educación académica
        var replaceResult = await _academicEducationService.ReplaceAcademicEducations(professionalResumeId, academicEducationEntities);
        return replaceResult.IsSuccess
            ? BaseResponse<bool>.Success(true)
            : BaseResponse<bool>.Fail("No se pudieron reemplazar las entradas de educación académica.", 400);
    }

    private async Task<BaseResponse<bool>> UpdateProfessionalResume(ProfessionalResumeResponse professionalResumeRequest, ProfessionalInfoRequest? professionalInfoRequest)
    {
        if (professionalInfoRequest == null)
            return BaseResponse<bool>.Success(true);

        // Crear la solicitud de actualización
        var updateRequest = new ProfessionalResumeUpdateRequest
        {
            ResumeId = professionalResumeRequest.ResumeId,
            ProfessionalSummary = professionalResumeRequest.ProfessionalSummary
        };

        // Verificar y asignar valores de Internship
        if (professionalInfoRequest.Internship != null)
        {
            updateRequest.IsInternshipCandidate = professionalInfoRequest.Internship.IsInternshipCandidate;
            updateRequest.InternshipTypeId = professionalInfoRequest.Internship.InternshipTypeId;
        }

        // Verificar y asignar valores de Inadeh
        if (professionalInfoRequest.Inadeh != null)
        {
            updateRequest.IsInadehCandidate = professionalInfoRequest.Inadeh.IsInadehCandidate;
            updateRequest.InadehCourseId = professionalInfoRequest.Inadeh.InadehCourseId;
        }

        // Llamar al servicio para actualizar el ProfessionalResume
        var updateResult = await _professionalResumeService.UpdateProfessionalResume(professionalResumeRequest.Id, updateRequest);

        return updateResult.IsSuccess
            ? BaseResponse<bool>.Success(true)
            : BaseResponse<bool>.Fail("No se pudo actualizar el ProfessionalResume", 400);
    }

    private async Task<List<ResumeResponse?>> MapResumesToResponses(IEnumerable<ResumeInfo?> resumes)
    {
        var resumeResponses = new List<ResumeResponse?>();

        foreach (var resume in resumes)
        {
            if (resume == null) continue;

            // Mapear el currículum base
            var response = _mapper.Map<ResumeResponse?>(resume);

            if (response != null)
            {
                // Mapear la información personal
                response.PersonalInfo = await MapPersonalInfo(resume.PersonalInfoId);

                // Verifica el tipo de currículum
                if (resume.ResumeTypeId == (int)ResumeTypeEnum.Professional)
                {
                    // Obtener el ProfessionalResume asociado al ResumeId
                    var professionalResumeResponse = await _professionalResumeService.GetProfessionalResumeByResumeId(resume.Id);
                    if (professionalResumeResponse.IsSuccess && professionalResumeResponse.Data != null)
                    {
                        // Inicializa ProfessionalInfo con las WorkExperiences directamente
                        response.ProfessionalInfo = new ProfessionalInfoResponse
                        {
                            Platzi = professionalResumeResponse.Data.Platzi
                        };
                    }
                }

                resumeResponses.Add(response);
            }
        }

        return resumeResponses;
    }

    private async Task<List<ResumeWithVacancyResponse?>> MapResumesToVacancyResponses(IEnumerable<ResumeWithVacancy?> resumes)
    {
        var resumeResponses = new List<ResumeWithVacancyResponse?>();

        foreach (var resume in resumes)
        {
            if (resume == null) continue;

            // Mapear el currículum base
            var response = _mapper.Map<ResumeWithVacancyResponse?>(resume);

            if (response != null)
            {
                // Mapear la información personal
                response.PersonalInfo = await MapPersonalInfo(resume.PersonalInfoId);

                resumeResponses.Add(response);
            }
        }

        return resumeResponses;
    }

    /// <summary>
    /// Obtiene el detalle de un currículum por datos de login (identityNumber, mobile, etc).
    /// </summary>
    /// <param name="request">Datos de login para buscar el currículum.</param>
    /// <returns>Una respuesta con el detalle del currículum.</returns>
    public async Task<BaseResponse<ResumeDetailResponse?>> GetResumeByLoginRequest(ResumeLoginRequest request)
    {
        // Buscar PersonalInfo por identityNumber y mobile
        var personalInfoResult = await _personalInfoService.GetPersonalInfoByIdentityNumberAndMobile(request.IdentityNumber, request.PhoneCountryCode, request.Mobile);
        if (!personalInfoResult.IsSuccess || personalInfoResult.Data == null)
            return BaseResponse<ResumeDetailResponse?>.Fail("Información personal no encontrada",404);

        // Buscar Resume por PersonalInfoId
        var resume = await _resumeRepository.GetResumeByPersonalInfoId(personalInfoResult.Data.Id);
        if (resume == null)
            return BaseResponse<ResumeDetailResponse?>.Fail("Currículum no encontrado",404);

        // Validar si ya ha sido descartado en alguna vacante de la empresa
        if (request.CompanyId.HasValue)
        {
            var existsStatusResponse = await _companyService.ExistsResumeStatusAsync(
                request.CompanyId.Value,
                resume.Id,
                (int)VacancyResumeStatusEnum.Descartado);
            if (existsStatusResponse.IsSuccess && existsStatusResponse.Data)
                return BaseResponse<ResumeDetailResponse?>.Fail("Usted ya ha participado con esta empresa.",403);
        }

        var response = _mapper.Map<ResumeDetailResponse>(resume);
        response.PersonalInfo = await MapPersonalInfo(resume.PersonalInfoId);
        response.Languages = await MapLanguages(resume.PersonalInfoId);

        if (resume.ResumeTypeId == (int)ResumeTypeEnum.Professional)
        {
            var professionalResumeResponse = await _professionalResumeService.GetProfessionalResumeByResumeId(resume.Id);
            if (professionalResumeResponse.IsSuccess && professionalResumeResponse.Data != null)
            {
                response.ProfessionalInfo = new ProfessionalInfoResponse
                {
                    WorkExperiences = await MapWorkExperiences(professionalResumeResponse.Data.Id),
                    SoftSkills = await MapSoftSkills(professionalResumeResponse.Data.Id),
                    TechnicalSkills = await MapTechnicalSkills(professionalResumeResponse.Data.Id),
                    Skills = await MapSkills(professionalResumeResponse.Data.Id),
                    AcademicEducations = await MapAcademicEducations(professionalResumeResponse.Data.Id),
                    Internship = professionalResumeResponse.Data.Internship,
                    Inadeh = professionalResumeResponse.Data.Inadeh,
                    Platzi = professionalResumeResponse.Data.Platzi
                };
            }
        }

        return BaseResponse<ResumeDetailResponse?>.Success(response);
    }
    #endregion
}
