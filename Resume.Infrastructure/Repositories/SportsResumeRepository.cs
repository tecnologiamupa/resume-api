using Dapper;
using Resume.Core.Entities;
using Resume.Core.RepositoryContracts;
using Resume.Infrastructure.DbContexts;

namespace Resume.Infrastructure.Repositories;

/// <summary>
/// Repositorio para manejar las operaciones relacionadas con los currículums deportivos.
/// </summary>
internal class SportsResumeRepository : ISportsResumeRepository
{
    private readonly ResumeDbContext _dbContext;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SportsResumeRepository"/>.
    /// </summary>
    /// <param name="dbContext">El contexto de base de datos utilizado para las operaciones.</param>
    public SportsResumeRepository(ResumeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Obtiene un currículum deportivo por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del currículum deportivo.</param>
    /// <returns>El currículum deportivo correspondiente o <c>null</c> si no se encuentra.</returns>
    public async Task<SportsResume?> GetSportsResumeById(Guid id)
    {
        string query = "SELECT * FROM `SportsResume` WHERE Id = @Id";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryFirstOrDefaultAsync<SportsResume>(query, new { Id = id });
        }
    }

    /// <summary>
    /// Obtiene un currículum deportivo asociado a un currículum general por el identificador del currículum general.
    /// </summary>
    /// <param name="resumeId">El identificador único del currículum general.</param>
    /// <returns>El currículum deportivo correspondiente o <c>null</c> si no se encuentra.</returns>
    public async Task<SportsResume?> GetSportsResumeByResumeId(Guid resumeId)
    {
        string query = "SELECT * FROM `SportsResume` WHERE ResumeId = @ResumeId";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryFirstOrDefaultAsync<SportsResume>(query, new { ResumeId = resumeId });
        }
    }

    /// <summary>
    /// Crea un nuevo currículum deportivo.
    /// </summary>
    /// <param name="sportsResume">El currículum deportivo a crear.</param>
    /// <returns>El currículum deportivo creado.</returns>
    /// <exception cref="Exception">Se lanza si no se puede crear el currículum deportivo.</exception>
    public async Task<SportsResume> CreateSportsResume(SportsResume sportsResume)
    {
        string query = @"
            INSERT INTO `SportsResume` (
                Id, ResumeId, SportsSummary, CreatedDate, CreatedBy
            ) VALUES (
                @Id, @ResumeId, @SportsSummary, @CreatedDate, @CreatedBy
            );
        ";

        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            int rowCountAffected = await connection.ExecuteAsync(query, sportsResume);
            if (rowCountAffected > 0)
            {
                return sportsResume;
            }
            else
            {
                throw new Exception("No se pudo crear el currículum deportivo.");
            }
        }
    }

    /// <summary>
    /// Actualiza un currículum deportivo existente.
    /// </summary>
    /// <param name="sportsResume">El currículum deportivo con los datos actualizados.</param>
    /// <returns><c>true</c> si la actualización fue exitosa; de lo contrario, <c>false</c>.</returns>
    public async Task<bool> UpdateSportsResume(SportsResume sportsResume)
    {
        string query = @"
            UPDATE `SportsResume`
            SET ResumeId = @ResumeId,
                SportsSummary = @SportsSummary,
                LastModifiedDate = @LastModifiedDate,
                LastModifiedBy = @LastModifiedBy
            WHERE Id = @Id;
        ";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            var affectedRows = await connection.ExecuteAsync(query, sportsResume);
            return affectedRows > 0;
        }
    }

    /// <summary>
    /// Elimina un currículum deportivo por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del currículum deportivo a eliminar.</param>
    /// <returns><c>true</c> si la eliminación fue exitosa; de lo contrario, <c>false</c>.</returns>
    public async Task<bool> DeleteSportsResume(Guid id)
    {
        string query = "DELETE FROM `SportsResume` WHERE Id = @Id";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }
    }
}