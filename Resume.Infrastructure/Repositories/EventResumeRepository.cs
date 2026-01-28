using Dapper;
using Resume.Core.DTOs;
using Resume.Core.Entities;
using Resume.Core.RepositoryContracts;
using Resume.Infrastructure.DbContexts;

namespace Resume.Infrastructure.Repositories;

/// <summary>
/// Repositorio para gestionar las operaciones relacionadas con la entidad <see cref="EventResume"/>.
/// </summary>
internal class EventResumeRepository : IEventResumeRepository
{
    private readonly ResumeDbContext _dbContext;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="EventResumeRepository"/>.
    /// </summary>
    /// <param name="dbContext">El contexto de base de datos utilizado para las operaciones.</param>
    public EventResumeRepository(ResumeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc/>
    public async Task<EventResume?> GetEventResumeByResumeId(Guid resumeId, int eventId)
    {
        string query = "SELECT * FROM `EventResume` WHERE ResumeId = @ResumeId AND EventId = @EventId";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryFirstOrDefaultAsync<EventResume>(query, new { ResumeId = resumeId, EventId = eventId });
        }
    }

    public async Task<(IEnumerable<EventResumeResponse> Items, int TotalRecords)> GetPagedEventResumesByFilter(
    EventResumeFilterRequest filter, int pageNumber, int pageSize)
    {
        int offset = (pageNumber - 1) * pageSize;

        var whereClauses = new List<string>();
        var parameters = new DynamicParameters();

        if (pageSize > 0)
        {
            parameters.Add("Offset", offset);
            parameters.Add("PageSize", pageSize);
        }

        if (filter?.EventId != null)
        {
            whereClauses.Add("er.EventId = @EventId");
            parameters.Add("EventId", filter.EventId);
        }
        if (filter?.ScheduleId != null)
        {
            whereClauses.Add("er.ScheduleId = @ScheduleId");
            parameters.Add("ScheduleId", filter.ScheduleId);
        }

        string whereSql = whereClauses.Count > 0 ? "WHERE " + string.Join(" AND ", whereClauses) : "";

        string limitOffsetSql = pageSize > 0 ? "LIMIT @PageSize OFFSET @Offset" : "";

        string query = $@"
        SELECT
            er.Id,
            er.ResumeId,
            er.ScheduleId,
            er.AddressDetail,
            er.EventId,
            er.CreatedDate,
            er.CreatedBy,
            er.LastModifiedDate,
            er.LastModifiedBy,
            pi.FirstName,
            pi.LastName,
            pi.IdentityNumber,
            pi.Email,
            pi.PhoneCountryCode,
            pi.Mobile,
            se.Name AS ScheduleName
        FROM EventResume er
        INNER JOIN Resume ri ON er.ResumeId = ri.Id
        INNER JOIN PersonalInfo pi ON ri.PersonalInfoId = pi.Id
        INNER JOIN ScheduleEvent se ON er.ScheduleId = se.Id
        {whereSql}
        ORDER BY er.CreatedDate DESC
        {limitOffsetSql};

        SELECT COUNT(*)
        FROM EventResume er
        INNER JOIN Resume ri ON er.ResumeId = ri.Id
        INNER JOIN PersonalInfo pi ON ri.PersonalInfoId = pi.Id
        INNER JOIN ScheduleEvent se ON er.ScheduleId = se.Id
        {whereSql};
    ";

        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            using (var multi = await connection.QueryMultipleAsync(query, parameters))
            {
                var items = await multi.ReadAsync<EventResumeResponse>();
                var totalRecords = await multi.ReadSingleAsync<int>();
                return (items, totalRecords);
            }
        }
    }

    /// <inheritdoc/>
    public async Task<EventResume> CreateEventResume(EventResume eventResume)
    {
        string query = @"
            INSERT INTO `EventResume` (
                ResumeId, ScheduleId, AddressDetail, EventId, CreatedDate, CreatedBy
            ) VALUES (
                @ResumeId, @ScheduleId, @AddressDetail, @EventId, @CreatedDate, @CreatedBy
            );
            SELECT LAST_INSERT_ID();
        ";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            var newId = await connection.ExecuteScalarAsync<int>(query, eventResume);
            eventResume.Id = newId;
            return eventResume;
        }
    }
}