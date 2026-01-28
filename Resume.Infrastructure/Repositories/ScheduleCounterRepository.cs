using Dapper;
using Resume.Core.Entities;
using Resume.Core.RepositoryContracts;
using Resume.Infrastructure.DbContexts;

namespace Resume.Infrastructure.Repositories;

internal class ScheduleCounterRepository : IScheduleCounterRepository
{
    private readonly ResumeDbContext _dbContext;

    public ScheduleCounterRepository(ResumeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ScheduleCounter?> GetScheduleCounterById(int id)
    {
        string query = "SELECT * FROM `ScheduleCounter` WHERE ScheduleId = @Id";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryFirstOrDefaultAsync<ScheduleCounter>(query, new { Id = id });
        }
    }
}
