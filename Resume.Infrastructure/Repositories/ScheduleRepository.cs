using Dapper;
using Resume.Core.Entities;
using Resume.Core.RepositoryContracts;
using Resume.Infrastructure.DbContexts;

namespace Resume.Infrastructure.Repositories;

/// <summary>
/// Repositorio para interactuar con los eventos programados en la base de datos.
/// </summary>
internal class ScheduleRepository : IScheduleRepository
{
    private readonly ResumeDbContext _dbContext;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ScheduleRepository"/>.
    /// </summary>
    /// <param name="dbContext">El contexto de base de datos utilizado para acceder a la base de datos.</param>
    public ScheduleRepository(ResumeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Obtiene una colección de eventos programados filtrados por el Id del evento.
    /// </summary>
    /// <param name="eventId">Id del evento por el cual filtrar.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado contiene una colección de eventos programados.</returns>
    public async Task<IEnumerable<ScheduleEvent?>> GetSchedulesByEventId(int eventId)
    {
        string query = "SELECT * FROM `ScheduleEvent` WHERE `EventName` = @EventId AND `IsActive` = 1";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryAsync<ScheduleEvent>(query, new { EventId = eventId });
        }
    }

    public async Task<ScheduleEvent?> GetScheduleById(int id)
    {
        string query = "SELECT * FROM `ScheduleEvent` WHERE Id = @Id";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryFirstOrDefaultAsync<ScheduleEvent>(query, new { Id = id });
        }
    }

    public async Task<ScheduleEvent> UpdateSchedule(ScheduleEvent scheduleEvent)
    {
        string query = @"
            UPDATE `ScheduleEvent` 
            SET `IsActive` = @IsActive
            WHERE `Id` = @Id;
            SELECT * FROM `ScheduleEvent` WHERE `Id` = @Id;";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            var updatedScheduleEvent = await connection.QuerySingleAsync<ScheduleEvent>(query, scheduleEvent);
            return updatedScheduleEvent;
        }
    }
}
