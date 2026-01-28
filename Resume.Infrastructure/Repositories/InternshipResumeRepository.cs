using Dapper;
using Resume.Core.Entities;
using Resume.Core.RepositoryContracts;
using Resume.Infrastructure.DbContexts;

namespace Resume.Infrastructure.Repositories;

/// <summary>
/// Repositorio para manejar las operaciones relacionadas con los currículums de prácticas.
/// </summary>
internal class InternshipResumeRepository : IInternshipResumeRepository
{
    private readonly ResumeDbContext _dbContext;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="InternshipResumeRepository"/>.
    /// </summary>
    /// <param name="dbContext">El contexto de base de datos utilizado para las operaciones.</param>
    public InternshipResumeRepository(ResumeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Obtiene un currículum de prácticas por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del currículum de prácticas.</param>
    /// <returns>El currículum de prácticas correspondiente o <c>null</c> si no se encuentra.</returns>
    public async Task<InternshipResume?> GetInternshipResumeById(Guid id)
    {
        string query = "SELECT * FROM `InternshipResume` WHERE Id = @Id";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryFirstOrDefaultAsync<InternshipResume>(query, new { Id = id });
        }
    }

    /// <summary>
    /// Obtiene un currículum de prácticas asociado a un currículum general por su identificador.
    /// </summary>
    /// <param name="resumeId">El identificador único del currículum general.</param>
    /// <returns>El currículum de prácticas correspondiente o <c>null</c> si no se encuentra.</returns>
    public async Task<InternshipResume?> GetInternshipResumeByResumeId(Guid resumeId)
    {
        string query = "SELECT * FROM `InternshipResume` WHERE ResumeId = @ResumeId";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryFirstOrDefaultAsync<InternshipResume>(query, new { ResumeId = resumeId });
        }
    }

    /// <summary>
    /// Crea un nuevo currículum de prácticas.
    /// </summary>
    /// <param name="internshipResume">El currículum de prácticas a crear.</param>
    /// <returns>El currículum de prácticas creado.</returns>
    /// <exception cref="Exception">Se lanza si no se puede crear el currículum de prácticas.</exception>
    public async Task<InternshipResume> CreateInternshipResume(InternshipResume internshipResume)
    {
        string query = @"
            INSERT INTO `InternshipResume` (
                Id, ResumeId, CareerObjective, CreatedDate, CreatedBy
            ) VALUES (
                @Id, @ResumeId, @CareerObjective, @CreatedDate, @CreatedBy
            );
        ";

        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            int rowCountAffected = await connection.ExecuteAsync(query, internshipResume);
            if (rowCountAffected > 0)
            {
                return internshipResume;
            }
            else
            {
                throw new Exception("No se pudo crear el currículum de pasantía.");
            }
        }
    }

    /// <summary>
    /// Actualiza un currículum de prácticas existente.
    /// </summary>
    /// <param name="internshipResume">El currículum de prácticas con los datos actualizados.</param>
    /// <returns><c>true</c> si la actualización fue exitosa; de lo contrario, <c>false</c>.</returns>
    public async Task<bool> UpdateInternshipResume(InternshipResume internshipResume)
    {
        string query = @"
            UPDATE `InternshipResume`
            SET ResumeId = @ResumeId,
                CareerObjective = @CareerObjective,
                LastModifiedDate = @LastModifiedDate,
                LastModifiedBy = @LastModifiedBy
            WHERE Id = @Id;
        ";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            var affectedRows = await connection.ExecuteAsync(query, internshipResume);
            return affectedRows > 0;
        }
    }

    /// <summary>
    /// Elimina un currículum de prácticas por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del currículum de prácticas a eliminar.</param>
    /// <returns><c>true</c> si la eliminación fue exitosa; de lo contrario, <c>false</c>.</returns>
    public async Task<bool> DeleteInternshipResume(Guid id)
    {
        string query = "DELETE FROM `InternshipResume` WHERE Id = @Id";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }
    }
}