using Dapper;
using Resume.Core.Entities;
using Resume.Core.RepositoryContracts;
using Resume.Infrastructure.DbContexts;

namespace Resume.Infrastructure.Repositories;

/// <summary>
/// Repositorio para gestionar las operaciones relacionadas con la entidad <see cref="PersonalInfo"/>.
/// </summary>
internal class PersonalInfoRepository : IPersonalInfoRepository
{
    private readonly ResumeDbContext _dbContext;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="PersonalInfoRepository"/>.
    /// </summary>
    /// <param name="dbContext">El contexto de base de datos utilizado para las operaciones.</param>
    public PersonalInfoRepository(ResumeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PersonalInfo?>> GetPersonalInfos()
    {
        string query = "SELECT * FROM `PersonalInfo`";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryAsync<PersonalInfo>(query);
        }
    }

    /// <inheritdoc/>
    public async Task<PersonalInfo?> GetPersonalInfoById(Guid id)
    {
        string query = "SELECT * FROM `PersonalInfo` WHERE Id = @Id";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryFirstOrDefaultAsync<PersonalInfo>(query, new { Id = id });
        }
    }

    /// <inheritdoc/>
    public async Task<PersonalInfo?> GetPersonalInfoByIdentityNumber(string identityNumber)
    {
        string query = "SELECT * FROM `PersonalInfo` WHERE IdentityNumber = @IdentityNumber";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryFirstOrDefaultAsync<PersonalInfo>(query, new { IdentityNumber = identityNumber });
        }
    }

    public async Task<PersonalInfo?> GetPersonalInfoByMobile(string mobile)
    {
        string query = "SELECT * FROM `PersonalInfo` WHERE Mobile = @Mobile";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryFirstOrDefaultAsync<PersonalInfo>(query, new { Mobile = mobile });
        }
    }

    /// <inheritdoc/>
    public async Task<PersonalInfo> CreatePersonalInfo(PersonalInfo personalInfo)
    {
        string query = @"
            INSERT INTO `PersonalInfo` (
                Id, ProfilePhotoUrl, FirstName, LastName, IdentityNumber, BirthDate, 
                Title, Email, PhoneCountryCode, Mobile, City, Country, 
                ProvinceId, DistrictId, TownshipId, GenderId, HasDisability, 
                DisabilityTypeId, DisabilityDescription, ScheduleId, CreatedDate, CreatedBy
            ) VALUES (
                @Id, @ProfilePhotoUrl, @FirstName, @LastName, @IdentityNumber, @BirthDate, 
                @Title, @Email, @PhoneCountryCode, @Mobile, @City, @Country, 
                @ProvinceId, @DistrictId, @TownshipId, @GenderId, @HasDisability, 
                @DisabilityTypeId, @DisabilityDescription, @ScheduleId, @CreatedDate, @CreatedBy
            );
        ";

        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            int rowCountAffected = await connection.ExecuteAsync(query, personalInfo);
            if (rowCountAffected > 0)
            {
                return personalInfo;
            }
            else
            {
                throw new Exception("No se pudo crear la información personal.");
            }
        }
    }

    /// <inheritdoc/>
    public async Task<bool> UpdatePersonalInfo(PersonalInfo personalInfo)
    {
        string query = @"
            UPDATE `PersonalInfo`
            SET ProfilePhotoUrl = @ProfilePhotoUrl,
                FirstName = @FirstName,
                LastName = @LastName,
                IdentityNumber = @IdentityNumber,
                BirthDate = @BirthDate,
                Title = @Title,
                Email = @Email,
                PhoneCountryCode = @PhoneCountryCode,
                Mobile = @Mobile,
                City = @City,
                Country = @Country,
                ProvinceId = @ProvinceId,
                DistrictId = @DistrictId,
                TownshipId = @TownshipId,
                GenderId = @GenderId,
                HasDisability = @HasDisability,
                DisabilityTypeId = @DisabilityTypeId,
                DisabilityDescription = @DisabilityDescription,
                LastModifiedDate = @LastModifiedDate,
                LastModifiedBy = @LastModifiedBy
            WHERE Id = @Id;
        ";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            var affectedRows = await connection.ExecuteAsync(query, personalInfo);
            return affectedRows > 0;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateProfilePhoto(PersonalInfo personalInfo)
    {
        string query = @"
            UPDATE `PersonalInfo`
            SET ProfilePhotoUrl = @ProfilePhotoUrl
            WHERE Id = @Id;
        ";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            var affectedRows = await connection.ExecuteAsync(query, personalInfo);
            return affectedRows > 0;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> DeletePersonalInfo(Guid id)
    {
        string query = "DELETE FROM `PersonalInfo` WHERE Id = @Id";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }
    }   

    /// <summary>
    /// Obtiene la información personal filtrando por número de identificación, código de país y móvil (comparando móviles limpios).
    /// </summary>
    /// <param name="identityNumber">El número de identificación.</param>
    /// <param name="phoneCountryCode">El código de país del teléfono.</param>
    /// <param name="mobile">El número de móvil.</param>
    /// <returns>La información personal que coincide con los criterios de búsqueda.</returns>
    public async Task<PersonalInfo?> GetPersonalInfoByIdentityNumberAndMobile(string identityNumber, string phoneCountryCode, string mobile)
    {
        string query = "SELECT * FROM `PersonalInfo` WHERE IdentityNumber = @IdentityNumber AND PhoneCountryCode = @PhoneCountryCode";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            var results = await connection.QueryAsync<PersonalInfo>(query, new { IdentityNumber = identityNumber, PhoneCountryCode = phoneCountryCode });
            var cleanedMobile = Resume.Core.Helpers.PhoneHelper.CleanMobile(mobile);
            return results.FirstOrDefault(p => Resume.Core.Helpers.PhoneHelper.CleanMobile(p.Mobile) == cleanedMobile);
        }
    }
}
