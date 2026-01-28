using Dapper;
using Resume.Core.Entities;
using Resume.Core.RepositoryContracts;
using Resume.Infrastructure.DbContexts;

namespace Resume.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de experiencias laborales.
/// </summary>
internal class WorkExperienceRepository : IWorkExperienceRepository
{
    private readonly ResumeDbContext _dbContext;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="WorkExperienceRepository"/>.
    /// </summary>
    /// <param name="dbContext">Contexto de base de datos.</param>
    public WorkExperienceRepository(ResumeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Obtiene todas las experiencias laborales asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <returns>Colección de experiencias laborales.</returns>
    public async Task<IEnumerable<WorkExperience>> GetWorkExperiencesByProfessionalResumeId(Guid professionalResumeId)
    {
        string query = "SELECT * FROM `WorkExperience` WHERE ProfessionalResumeId = @ProfessionalResumeId";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryAsync<WorkExperience>(query, new { ProfessionalResumeId = professionalResumeId });
        }
    }
    
    /// <summary>
    /// Elimina todas las experiencias laborales asociadas a un currículum profesional y crea nuevas experiencias laborales en una sola transacción.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <param name="workExperiences">Colección de experiencias laborales a crear.</param>
    /// <returns>True si ambas operaciones fueron exitosas; de lo contrario, false.</returns>
    public async Task<bool> ReplaceWorkExperiences(Guid professionalResumeId, IEnumerable<WorkExperience> workExperiences)
    {
        if (workExperiences == null)
        {
            throw new ArgumentException("La lista de experiencias laborales no puede ser nula.", nameof(workExperiences));
        }

        string deleteQuery = "DELETE FROM `WorkExperience` WHERE ProfessionalResumeId = @ProfessionalResumeId";
        string insertQuery = @"
        INSERT INTO `WorkExperience` (
            ProfessionalResumeId, Company, Position, StartDate, EndDate, CurrentlyWorking, PositionDescription, CreatedDate, CreatedBy
        ) VALUES (
            @ProfessionalResumeId, @Company, @Position, @StartDate, @EndDate, @CurrentlyWorking, @PositionDescription, @CreatedDate, @CreatedBy
        );
    ";

        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // Eliminar todas las experiencias laborales asociadas al ProfessionalResumeId
                    await connection.ExecuteAsync(deleteQuery, new { ProfessionalResumeId = professionalResumeId }, transaction);

                    // Crear nuevas experiencias laborales
                    if (workExperiences.Any())
                    {
                        int rowsAffected = await connection.ExecuteAsync(insertQuery, workExperiences, transaction);

                        if (rowsAffected != workExperiences.Count())
                        {
                            throw new Exception("No se pudieron crear todas las experiencias laborales.");
                        }
                    }

                    // Confirmar la transacción
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    // Revertir la transacción en caso de error
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}