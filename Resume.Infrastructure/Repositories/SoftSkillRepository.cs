using Dapper;
using Resume.Core.Entities;
using Resume.Core.RepositoryContracts;
using Resume.Infrastructure.DbContexts;

namespace Resume.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de habilidades blandas.
/// </summary>
internal class SoftSkillRepository : ISoftSkillRepository
{
    private readonly ResumeDbContext _dbContext;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="SoftSkillRepository"/>.
    /// </summary>
    /// <param name="dbContext">Contexto de base de datos.</param>
    public SoftSkillRepository(ResumeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Obtiene todas las habilidades blandas asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <returns>Colección de habilidades blandas.</returns>
    public async Task<IEnumerable<SoftSkill>> GetSoftSkillsByProfessionalResumeId(Guid professionalResumeId)
    {
        string query = "SELECT * FROM `SoftSkill` WHERE ProfessionalResumeId = @ProfessionalResumeId";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryAsync<SoftSkill>(query, new { ProfessionalResumeId = professionalResumeId });
        }
    }

    /// <summary>
    /// Elimina todas las habilidades blandas asociadas a un currículum profesional y crea nuevas habilidades blandas en una sola transacción.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <param name="softSkills">Colección de habilidades blandas a crear.</param>
    /// <returns>True si ambas operaciones fueron exitosas; de lo contrario, false.</returns>
    public async Task<bool> ReplaceSoftSkills(Guid professionalResumeId, IEnumerable<SoftSkill> softSkills)
    {
        if (softSkills == null)
        {
            throw new ArgumentException("La lista de habilidades blandas no puede ser nula.", nameof(softSkills));
        }

        string deleteQuery = "DELETE FROM `SoftSkill` WHERE ProfessionalResumeId = @ProfessionalResumeId";
        string insertQuery = @"
        INSERT INTO `SoftSkill` (
            ProfessionalResumeId, Skill
        ) VALUES (
            @ProfessionalResumeId, @Skill
        );
    ";

        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // Eliminar todas las habilidades blandas asociadas al ProfessionalResumeId
                    await connection.ExecuteAsync(deleteQuery, new { ProfessionalResumeId = professionalResumeId }, transaction);

                    // Crear nuevas habilidades blandas
                    if (softSkills.Any())
                    {
                        int rowsAffected = await connection.ExecuteAsync(insertQuery, softSkills, transaction);

                        if (rowsAffected != softSkills.Count())
                        {
                            throw new Exception("No se pudieron crear todas las habilidades blandas.");
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