using Dapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;
using Resume.Core.RepositoryContracts;
using Resume.Infrastructure.DbContexts;

namespace Resume.Infrastructure.Repositories;

/// <summary>
/// Repositorio para la gestión de currículums utilizando Dapper.
/// </summary>
internal class ResumeRepository : IResumeRepository
{
    private readonly ResumeDbContext _dbContext;
    private readonly CompanyRecruitmentDbContext _companyRecruitmentDbContext;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ResumeRepository"/>.
    /// </summary>
    /// <param name="dbContext">El contexto de base de datos utilizado para las operaciones.</param>
    public ResumeRepository(ResumeDbContext dbContext, CompanyRecruitmentDbContext companyRecruitmentDbContext)
    {
        _dbContext = dbContext;
        _companyRecruitmentDbContext = companyRecruitmentDbContext;
    }

    /// <summary>
    /// Obtiene una lista de todos los currículums disponibles.
    /// </summary>
    /// <returns>Una tarea que representa una colección de objetos <see cref="ResumeInfo"/>.</returns>
    public async Task<IEnumerable<ResumeInfo?>> GetResumes()
    {
        string query = "SELECT * FROM `Resume` WHERE PersonalInfoId IS NOT NULL";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryAsync<ResumeInfo>(query);
        }
    }

    /// <summary>
    /// Obtiene una lista paginada de currículums que tienen el campo PersonalInfoId con valor.
    /// </summary>
    /// <param name="pageNumber">Número de página a recuperar.</param>
    /// <param name="pageSize">Cantidad de elementos por página.</param>
    /// <returns>Una tupla que contiene la colección de currículums y el total de registros.</returns>
    public async Task<(IEnumerable<ResumeInfo?> Resumes, int TotalRecords)> GetPagedResumes(int pageNumber, int pageSize)
    {
        // Calcula el offset para la paginación
        int offset = (pageNumber - 1) * pageSize;

        string query = @"
            SELECT 
                r.*
            FROM `Resume` r
            WHERE r.PersonalInfoId IS NOT NULL
            ORDER BY r.CreatedDate DESC
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*) FROM `Resume` WHERE PersonalInfoId IS NOT NULL;
        ";

        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            // Ejecuta ambas consultas en una sola llamada
            using (var multi = await connection.QueryMultipleAsync(query, new { Offset = offset, PageSize = pageSize }))
            {
                var resumes = await multi.ReadAsync<ResumeInfo>();
                var totalRecords = await multi.ReadSingleAsync<int>();
                return (resumes, totalRecords);
            }
        }
    }

    public async Task<(IEnumerable<ResumeInfo?> Resumes, int TotalRecords)> GetPagedResumesByFilter(ResumeFilterRequest filter, int pageNumber, int pageSize)
    {
        int offset = (pageNumber - 1) * pageSize;

        var whereClauses = new List<string>
    {
        "r.PersonalInfoId IS NOT NULL"
    };
        var parameters = new DynamicParameters();
        parameters.Add("Offset", offset);
        parameters.Add("PageSize", pageSize);

        bool joinPersonalInfo = false;
        bool joinProfessionalResume = false;

        // Filtro por lista de números de identidad
        if (filter?.IdentityNumbers != null && filter.IdentityNumbers.Any())
        {
            joinPersonalInfo = true;
            whereClauses.Add("pi.IdentityNumber IN @IdentityNumbers");
            parameters.Add("IdentityNumbers", filter.IdentityNumbers);
        }

        if (!string.IsNullOrWhiteSpace(filter?.IdentityNumber))
        {
            joinPersonalInfo = true;
            whereClauses.Add("pi.IdentityNumber = @IdentityNumber");
            parameters.Add("IdentityNumber", filter.IdentityNumber);
        }
        if (!string.IsNullOrWhiteSpace(filter?.Mobile))
        {
            joinPersonalInfo = true;
            whereClauses.Add("pi.Mobile = @Mobile");
            parameters.Add("Mobile", filter.Mobile);
        }
        if (!string.IsNullOrWhiteSpace(filter?.FirstName))
        {
            joinPersonalInfo = true;
            whereClauses.Add("pi.FirstName LIKE CONCAT('%', @FirstName, '%')");
            parameters.Add("FirstName", filter.FirstName);
        }
        if (!string.IsNullOrWhiteSpace(filter?.LastName))
        {
            joinPersonalInfo = true;
            whereClauses.Add("pi.LastName LIKE CONCAT('%', @LastName, '%')");
            parameters.Add("LastName", filter.LastName);
        }
        if (!string.IsNullOrWhiteSpace(filter?.SearchText))
        {
            joinPersonalInfo = true;
            var searchWords = filter.SearchText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var fullNameConditions = searchWords
                .Select((w, i) => $"CONCAT(pi.FirstName, ' ', pi.LastName) LIKE CONCAT('%', @SearchTextWord{i}, '%')")
                .ToList();
            for (int i = 0; i < searchWords.Length; i++)
            {
                parameters.Add($"SearchTextWord{i}", searchWords[i]);
            }
            var otherConditions = @"
            pi.IdentityNumber LIKE CONCAT('%', @SearchText, '%') OR
            pi.Mobile LIKE CONCAT('%', @SearchText, '%') OR
            pi.Title LIKE CONCAT('%', @SearchText, '%')
        ";
            whereClauses.Add($@"(
            {otherConditions} OR
            ({string.Join(" AND ", fullNameConditions)})
        )");
            parameters.Add("SearchText", filter.SearchText);
        }

        // Filtro HasDisability (PersonalInfo)
        if (filter?.HasDisability != null)
        {
            joinPersonalInfo = true;
            whereClauses.Add("pi.HasDisability = @HasDisability");
            parameters.Add("HasDisability", filter.HasDisability);
        }

        // Filtro IsInternshipCandidate (ProfessionalResume)
        if (filter?.IsInternshipCandidate != null)
        {
            joinProfessionalResume = true;
            whereClauses.Add("pr.IsInternshipCandidate = @IsInternshipCandidate");
            parameters.Add("IsInternshipCandidate", filter.IsInternshipCandidate);
        }

        // Filtro IsInadehCandidate (ProfessionalResume)
        if (filter?.IsInadehCandidate != null)
        {
            joinProfessionalResume = true;
            whereClauses.Add("pr.IsInadehCandidate = @IsInadehCandidate");
            parameters.Add("IsInadehCandidate", filter.IsInadehCandidate);
        }

        // Filtro IsPlatziAssigned (ProfessionalResume)
        if (filter?.IsPlatziAssigned != null)
        {
            joinProfessionalResume = true;
            whereClauses.Add("pr.IsPlatziAssigned = @IsPlatziAssigned");
            parameters.Add("IsPlatziAssigned", filter.IsPlatziAssigned);
        }

        // Filtros de favoritos y status
        List<Guid> favoriteResumeIds = null;
        bool filterFavorites = filter?.CompanyId != null || filter?.VacancyId != null;
        if (filterFavorites)
        {
            using (var favoriteConnection = await _companyRecruitmentDbContext.GetOpenConnectionAsync())
            {
                string favoriteQuery = "SELECT DISTINCT ResumeId FROM FavoriteVacancyResume WHERE 1=1";
                var favoriteParams = new DynamicParameters();

                if (filter?.CompanyId != null)
                {
                    favoriteQuery += " AND CompanyId = @CompanyId";
                    favoriteParams.Add("CompanyId", filter.CompanyId);
                }
                if (filter?.VacancyId != null)
                {
                    favoriteQuery += " AND VacancyId = @VacancyId";
                    favoriteParams.Add("VacancyId", filter.VacancyId);
                }
                if (filter?.VacancyResumeStatusId != null)
                {
                    favoriteQuery += @"
                 AND ResumeId IN (
                    SELECT ResumeId FROM VacancyResumeStatus
                    WHERE StatusId = @StatusId";
                    if (filter?.CompanyId != null)
                    {
                        favoriteQuery += " AND CompanyId = @CompanyId";
                    }
                    if (filter?.VacancyId != null)
                    {
                        favoriteQuery += " AND VacancyId = @VacancyId";
                    }
                    favoriteQuery += ")";
                    favoriteParams.Add("StatusId", filter.VacancyResumeStatusId);
                }

                favoriteResumeIds = (await favoriteConnection.QueryAsync<Guid>(favoriteQuery, favoriteParams)).ToList();
            }

            if (favoriteResumeIds.Count == 0)
            {
                return (Enumerable.Empty<ResumeInfo>(), 0);
            }

            whereClauses.Add("r.Id IN @FavoriteResumeIds");
            parameters.Add("FavoriteResumeIds", favoriteResumeIds);
        }

        // Construcción de los JOINs
        string joinSql = "";
        if (joinPersonalInfo)
            joinSql += "INNER JOIN PersonalInfo pi ON pi.Id = r.PersonalInfoId ";
        if (joinProfessionalResume)
            joinSql += "INNER JOIN ProfessionalResume pr ON pr.ResumeId = r.Id ";

        string whereSql = whereClauses.Count > 0 ? "WHERE " + string.Join(" AND ", whereClauses) : "";

        string query = $@"
        SELECT DISTINCT
            r.*
        FROM `Resume` r
        {joinSql}
        {whereSql}
        ORDER BY r.CreatedDate DESC
        LIMIT @PageSize OFFSET @Offset;

        SELECT COUNT(DISTINCT r.Id) 
        FROM `Resume` r
        {joinSql}
        {whereSql};
    ";

        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            using (var multi = await connection.QueryMultipleAsync(query, parameters))
            {
                var resumes = await multi.ReadAsync<ResumeInfo>();
                var totalRecords = await multi.ReadSingleAsync<int>();
                return (resumes, totalRecords);
            }
        }
    }

    /// <summary>
    /// Obtiene una lista paginada de currículums completados.
    /// </summary>
    /// <param name="pageNumber">Número de página a recuperar.</param>
    /// <param name="pageSize">Cantidad de elementos por página.</param>
    /// <returns>Una tupla que contiene la colección de currículums completados y el total de registros.</returns>
    public async Task<(IEnumerable<ResumeInfo?> Resumes, int TotalRecords)> GetCompletedResumesPaged(int pageNumber, int pageSize)
    {
        int offset = (pageNumber - 1) * pageSize;

        string query = @"
        WITH CompletedResumes AS (
            SELECT r.*
            FROM `Resume` r
            INNER JOIN `ProfessionalResume` pr ON pr.ResumeId = r.Id
            WHERE r.PersonalInfoId IS NOT NULL
              AND EXISTS (SELECT 1 FROM `WorkExperience` we WHERE we.ProfessionalResumeId = pr.Id)
              AND EXISTS (SELECT 1 FROM `AcademicEducation` ae WHERE ae.ProfessionalResumeId = pr.Id)
              AND (
                  EXISTS (SELECT 1 FROM `SoftSkill` ss WHERE ss.ProfessionalResumeId = pr.Id) OR
                  EXISTS (SELECT 1 FROM `TechnicalSkill` ts WHERE ts.ProfessionalResumeId = pr.Id) OR
                  EXISTS (SELECT 1 FROM `PersonalLanguage` pl WHERE pl.PersonalInfoId = r.PersonalInfoId)
              )
              AND r.Summary IS NOT NULL
        )
        SELECT 
            r.*, 
            (SELECT COUNT(*) FROM CompletedResumes) AS TotalRecords
        FROM CompletedResumes r
        ORDER BY r.CreatedDate DESC
        LIMIT @PageSize OFFSET @Offset;
    ";

        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            var results = await connection.QueryAsync<ResumeInfo, long, (ResumeInfo, long)>(
                query,
                (resume, totalRecords) => (resume, totalRecords),
                new { Offset = offset, PageSize = pageSize },
                splitOn: "TotalRecords"
            );

            var resumes = results.Select(r => r.Item1).ToList();
            var totalRecords = (int)results.FirstOrDefault().Item2;

            return (resumes, totalRecords);
        }
    }

    /// <summary>
    /// Obtiene una lista paginada de currículums incompletos.
    /// </summary>
    /// <param name="pageNumber">Número de página a recuperar.</param>
    /// <param name="pageSize">Cantidad de elementos por página.</param>
    /// <returns>Una tupla que contiene la colección de currículums incompletos y el total de registros.</returns>
    public async Task<(IEnumerable<ResumeInfo?> Resumes, int TotalRecords)> GetIncompleteResumesPaged(int pageNumber, int pageSize)
    {
        int offset = (pageNumber - 1) * pageSize;

        string query = @"
        WITH IncompleteResumes AS (
            SELECT r.*
            FROM `Resume` r
            INNER JOIN `ProfessionalResume` pr ON pr.ResumeId = r.Id
            WHERE r.PersonalInfoId IS NOT NULL
              AND (
                  NOT EXISTS (SELECT 1 FROM `WorkExperience` we WHERE we.ProfessionalResumeId = pr.Id)
                  OR NOT EXISTS (SELECT 1 FROM `AcademicEducation` ae WHERE ae.ProfessionalResumeId = pr.Id)
                  OR (
                      NOT EXISTS (SELECT 1 FROM `SoftSkill` ss WHERE ss.ProfessionalResumeId = pr.Id) AND
                      NOT EXISTS (SELECT 1 FROM `TechnicalSkill` ts WHERE ts.ProfessionalResumeId = pr.Id) AND
                      NOT EXISTS (SELECT 1 FROM `PersonalLanguage` pl WHERE pl.PersonalInfoId = r.PersonalInfoId)
                  )
                  OR r.Summary IS NULL
              )
        )
        SELECT 
            r.*, 
            (SELECT COUNT(*) FROM IncompleteResumes) AS TotalRecords
        FROM IncompleteResumes r
        ORDER BY r.CreatedDate DESC
        LIMIT @PageSize OFFSET @Offset;
    ";

        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            var results = await connection.QueryAsync<ResumeInfo, long, (ResumeInfo, long)>(
                query,
                (resume, totalRecords) => (resume, totalRecords),
                new { Offset = offset, PageSize = pageSize },
                splitOn: "TotalRecords"
            );

            var resumes = results.Select(r => r.Item1).ToList();
            var totalRecords = (int)results.FirstOrDefault().Item2;

            return (resumes, totalRecords);
        }
    }

    /// <summary>
    /// Obtiene una lista de currículums favoritos por empresa, permitiendo duplicados por vacante y mostrando el nombre de la vacante.
    /// </summary>
    /// <param name="companyId">El identificador de la empresa.</param>
    /// <returns>Una colección de currículums con el nombre de la vacante.</returns>
    public async Task<IEnumerable<ResumeWithVacancy>> GetResumesByCompany(Guid companyId)
    {
        // 1. Obtener los favoritos con duplicados y nombre de vacante
        string favoritesQuery = @"
        SELECT 
            fvr.ResumeId,
            v.JobTitle AS VacancyName
        FROM FavoriteVacancyResume fvr
        INNER JOIN Vacancy v ON v.Id = fvr.VacancyId
        WHERE fvr.CompanyId = @CompanyId
    ";

        List<(Guid ResumeId, string VacancyName)> favoriteResumes;
        using (var favoriteConnection = await _companyRecruitmentDbContext.GetOpenConnectionAsync())
        {
            favoriteResumes = (await favoriteConnection.QueryAsync<(Guid, string)>(favoritesQuery, new { CompanyId = companyId })).ToList();
        }

        if (!favoriteResumes.Any())
            return Enumerable.Empty<ResumeWithVacancy>();

        // 2. Obtener los datos de los resumes y su PersonalInfo desde ResumeDbContext
        var resumeIds = favoriteResumes.Select(x => x.ResumeId).ToList();

        string resumesQuery = @"
            SELECT 
                r.Id,
                r.ResumeTypeId,
                r.PersonalInfoId,
                r.LinkedIn,
                r.PortfolioUrl,
                r.Summary,
                -- PersonalInfo fields
                pi.Id as PersonalInfo_Id,
                pi.Id,
                pi.ProfilePhotoUrl,
                pi.FirstName,
                pi.LastName,
                pi.IdentityNumber,
                pi.BirthDate,
                pi.Title,
                pi.Email,
                pi.PhoneCountryCode,
                pi.Mobile,
                pi.City,
                pi.Country,
                pi.ProvinceId,
                pi.DistrictId,
                pi.TownshipId,
                pi.GenderId,
                pi.HasDisability,
                pi.DisabilityTypeId,
                pi.DisabilityDescription,
                pi.ScheduleId,
                pi.CreatedDate,
                pi.CreatedBy,
                pi.LastModifiedDate,
                pi.LastModifiedBy
            FROM Resume r
            INNER JOIN PersonalInfo pi ON pi.Id = r.PersonalInfoId
            WHERE r.Id IN @ResumeIds AND r.PersonalInfoId IS NOT NULL
        ";

        Dictionary<Guid, (ResumeInfo Resume, PersonalInfo PersonalInfo)> resumesDict;
        using (var resumeConnection = await _dbContext.GetOpenConnectionAsync())
        {
            var resumes = await resumeConnection.QueryAsync<ResumeInfo, PersonalInfo, (ResumeInfo, PersonalInfo)>(
                resumesQuery,
                (resume, personalInfo) => (resume, personalInfo),
                new { ResumeIds = resumeIds },
                splitOn: "PersonalInfo_Id"
            );
            resumesDict = resumes.ToDictionary(r => r.Item1.Id, r => r);
        }

        // 3. Unir los datos y retornar el DTO
        var result = favoriteResumes
            .Where(x => resumesDict.ContainsKey(x.ResumeId))
            .Select(x =>
            {
                var (resume, personalInfo) = resumesDict[x.ResumeId];
                return new ResumeWithVacancy
                {
                    Id = resume.Id,
                    ResumeTypeId = resume.ResumeTypeId,
                    PersonalInfoId = resume.PersonalInfoId,
                    LinkedIn = resume.LinkedIn,
                    PortfolioUrl = resume.PortfolioUrl,
                    Summary = resume.Summary,
                    VacancyName = x.VacancyName,
                    PersonalInfo = personalInfo
                };
            })
            .ToList();

        return result;
    }

    /// <summary>
    /// Obtiene un currículum específico por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del currículum.</param>
    /// <returns>Una tarea que representa el objeto <see cref="ResumeInfo"/> correspondiente, o null si no se encuentra.</returns>
    public async Task<ResumeInfo?> GetResumeById(Guid id)
    {
        string query = "SELECT * FROM `Resume` WHERE Id = @Id";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryFirstOrDefaultAsync<ResumeInfo>(query, new { Id = id });
        }
    }

    /// <summary>
    /// Obtiene un currículum específico por el identificador único de la información personal asociada.
    /// </summary>
    /// <param name="personalInfoId">El identificador único de la información personal.</param>
    /// <returns>Una tarea que representa el objeto <see cref="ResumeInfo"/> correspondiente, o null si no se encuentra.</returns>
    public async Task<ResumeInfo?> GetResumeByPersonalInfoId(Guid personalInfoId)
    {
        string query = "SELECT * FROM `Resume` WHERE PersonalInfoId = @PersonalInfoId";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryFirstOrDefaultAsync<ResumeInfo>(query, new { PersonalInfoId = personalInfoId });
        }
    }

    /// <summary>
    /// Crea un nuevo currículum en el repositorio.
    /// </summary>
    /// <param name="resume">El objeto <see cref="ResumeInfo"/> que representa el currículum a crear.</param>
    /// <returns>Una tarea que representa el currículum creado.</returns>
    /// <exception cref="Exception">Se lanza si no se puede crear el currículum.</exception>
    public async Task<ResumeInfo> CreateResume(ResumeInfo resume)
    {
        string query = @"
            INSERT INTO `Resume` (
                Id, ResumeTypeId, PersonalInfoId, LinkedIn, PortfolioUrl, Summary, CreatedDate, CreatedBy
            ) VALUES (
                @Id, @ResumeTypeId, @PersonalInfoId, @LinkedIn, @PortfolioUrl, @Summary, @CreatedDate, @CreatedBy
            );
        ";

        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            int rowCountAffected = await connection.ExecuteAsync(query, resume);
            if (rowCountAffected > 0)
            {
                return resume;
            }
            else
            {
                throw new Exception("No se pudo crear el currículum.");
            }
        }
    }

    /// <summary>
    /// Actualiza un currículum existente en el repositorio.
    /// </summary>
    /// <param name="resume">El objeto <see cref="ResumeInfo"/> que contiene los datos actualizados del currículum.</param>
    /// <returns>Una tarea que representa un valor booleano indicando si la actualización fue exitosa.</returns>
    public async Task<bool> UpdateResume(ResumeInfo resume)
    {
        string query = @"
            UPDATE `Resume`
            SET ResumeTypeId = @ResumeTypeId,
                PersonalInfoId = @PersonalInfoId,
                LinkedIn = @LinkedIn,
                PortfolioUrl = @PortfolioUrl,
                Summary = @Summary,
                LastModifiedDate = @LastModifiedDate,
                LastModifiedBy = @LastModifiedBy
            WHERE Id = @Id;
        ";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            var affectedRows = await connection.ExecuteAsync(query, resume);
            return affectedRows > 0;
        }
    }

    /// <summary>
    /// Elimina un currículum del repositorio por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del currículum a eliminar.</param>
    /// <returns>Una tarea que representa un valor booleano indicando si la eliminación fue exitosa.</returns>
    public async Task<bool> DeleteResume(Guid id)
    {
        string query = "DELETE FROM `Resume` WHERE Id = @Id";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }
    }    
}
