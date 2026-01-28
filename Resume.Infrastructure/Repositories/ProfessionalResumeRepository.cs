using Dapper;
using Resume.Core.Entities;
using Resume.Core.RepositoryContracts;
using Resume.Infrastructure.DbContexts;

namespace Resume.Infrastructure.Repositories;

/// <summary>
/// Repositorio para manejar las operaciones relacionadas con la entidad <see cref="ProfessionalResume"/>.
/// Implementa la interfaz <see cref="IProfessionalResumeRepository"/>.
/// </summary>
internal class ProfessionalResumeRepository : IProfessionalResumeRepository
{
    private readonly ResumeDbContext _dbContext;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ProfessionalResumeRepository"/>.
    /// </summary>
    /// <param name="dbContext">El contexto de base de datos utilizado para las operaciones.</param>
    public ProfessionalResumeRepository(ResumeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Obtiene un currículum profesional por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del currículum profesional.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado contiene el currículum profesional si se encuentra; de lo contrario, <c>null</c>.</returns>
    public async Task<ProfessionalResume?> GetProfessionalResumeById(Guid id)
    {
        string query = "SELECT * FROM `ProfessionalResume` WHERE Id = @Id";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryFirstOrDefaultAsync<ProfessionalResume>(query, new { Id = id });
        }
    }

    /// <summary>
    /// Obtiene un currículum profesional asociado a un currículum específico por su identificador.
    /// </summary>
    /// <param name="resumeId">El identificador único del currículum asociado.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado contiene el currículum profesional si se encuentra; de lo contrario, <c>null</c>.</returns>
    public async Task<ProfessionalResume?> GetProfessionalResumeByResumeId(Guid resumeId)
    {
        string query = "SELECT * FROM `ProfessionalResume` WHERE ResumeId = @ResumeId";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryFirstOrDefaultAsync<ProfessionalResume>(query, new { ResumeId = resumeId });
        }
    }

    /// <summary>
    /// Crea un nuevo currículum profesional.
    /// </summary>
    /// <param name="professionalResume">La entidad <see cref="ProfessionalResume"/> que se va a crear.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado contiene la entidad creada.</returns>
    public async Task<ProfessionalResume> CreateProfessionalResume(ProfessionalResume professionalResume)
    {
        string query = @"
            INSERT INTO `ProfessionalResume` (
                Id, ResumeId, ProfessionalSummary, CreatedDate, CreatedBy
            ) VALUES (
                @Id, @ResumeId, @ProfessionalSummary, @CreatedDate, @CreatedBy
            );
        ";

        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            int rowCountAffected = await connection.ExecuteAsync(query, professionalResume);
            if (rowCountAffected > 0)
            {
                return professionalResume;
            }
            else
            {
                throw new Exception("No se pudo crear el currículum profesional.");
            }
        }
    }

    /// <summary>
    /// Actualiza un currículum profesional existente.
    /// </summary>
    /// <param name="professionalResume">La entidad <see cref="ProfessionalResume"/> con los datos actualizados.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado es <c>true</c> si la actualización fue exitosa; de lo contrario, <c>false</c>.</returns>
    public async Task<bool> UpdateProfessionalResume(ProfessionalResume professionalResume)
    {
        string query = @"
            UPDATE `ProfessionalResume`
            SET ResumeId = @ResumeId,
                ProfessionalSummary = @ProfessionalSummary,
                IsInternshipCandidate = @IsInternshipCandidate,
                InternshipTypeId = @InternshipTypeId,
                IsInadehCandidate = @IsInadehCandidate,
                InadehCourseId = @InadehCourseId,
                LastModifiedDate = @LastModifiedDate,
                LastModifiedBy = @LastModifiedBy
            WHERE Id = @Id;
        ";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            var affectedRows = await connection.ExecuteAsync(query, professionalResume);
            return affectedRows > 0;
        }
    }

    /// <summary>
    /// Actualiza en lote los campos IsPlatziAssigned, PlatziCompanyUserId y PlatziUserId de una lista de ProfessionalResume, basado en ResumeId.
    /// </summary>
    /// <param name="resumes">Lista de objetos ProfessionalResume con los datos a actualizar.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado es <c>true</c> si al menos una actualización fue exitosa; de lo contrario, <c>false</c>.</returns>
    public async Task<bool> UpdatePlatziFieldsByResumeListAsync(IEnumerable<ProfessionalResume> resumes)
    {
        string query = @"
        UPDATE `ProfessionalResume`
        SET IsPlatziAssigned = @IsPlatziAssigned,
            PlatziCompanyUserId = @PlatziCompanyUserId,
            PlatziUserId = @PlatziUserId,
            LastModifiedDate = @LastModifiedDate,
            LastModifiedBy = @LastModifiedBy
        WHERE ResumeId = @ResumeId;
    ";

        using (var connection = await _dbContext.GetOpenConnectionAsync())
        using (var transaction = connection.BeginTransaction())
        {
            int totalAffected = 0;
            foreach (var resume in resumes)
            {
                var parameters = new
                {
                    IsPlatziAssigned = resume.IsPlatziAssigned,
                    PlatziCompanyUserId = resume.PlatziCompanyUserId,
                    PlatziUserId = resume.PlatziUserId,
                    LastModifiedDate = resume.LastModifiedDate,
                    LastModifiedBy = resume.LastModifiedBy,
                    ResumeId = resume.ResumeId
                };

                totalAffected += await connection.ExecuteAsync(query, parameters, transaction);
            }

            if (totalAffected > 0)
            {
                transaction.Commit();
                return true;
            }
            else
            {
                transaction.Rollback();
                return false;
            }
        }
    }

    /// <summary>
    /// Elimina un currículum profesional por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del currículum profesional a eliminar.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado es <c>true</c> si la eliminación fue exitosa; de lo contrario, <c>false</c>.</returns>
    public async Task<bool> DeleteProfessionalResume(Guid id)
    {
        string query = "DELETE FROM `ProfessionalResume` WHERE Id = @Id";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }
    }
}