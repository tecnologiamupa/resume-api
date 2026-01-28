using Dapper;
using Resume.Core.Entities;
using Resume.Core.RepositoryContracts;
using Resume.Infrastructure.DbContexts;

namespace Resume.Infrastructure.Repositories;

internal class ApiLogRepository : IApiLogRepository
{
    private readonly ResumeDbContext _dbContext;

    public ApiLogRepository(ResumeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task InsertApiLog(ApiLog logEntry)
    {
        string query = @"
        INSERT INTO `ApiLogs`
        (Timestamp, Level, Message, Exception, RequestPath, HttpMethod, IpAddress, UserAgent, Referer, RequestBody)
        VALUES
        (@Timestamp, @Level, @Message, @Exception, @RequestPath, @HttpMethod, @IpAddress, @UserAgent, @Referer, @RequestBody);
    ";

        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            await connection.ExecuteAsync(query, logEntry);
        }
    }
}
