using Dapper;
using Resume.Core.Entities;
using Resume.Core.RepositoryContracts;
using Resume.Infrastructure.DbContexts;

namespace Resume.Infrastructure.Repositories;

internal class ProfessionalSkillRepository : IProfessionalSkillRepository
{
    private readonly ResumeDbContext _dbContext;

    public ProfessionalSkillRepository(ResumeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Obtiene todas las habilidades profesionales asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <returns>Colección de habilidades profesionales.</returns>
    public async Task<IEnumerable<ProfessionalSkill>> GetSkillsByProfessionalResumeId(Guid professionalResumeId)
    {
        string query = "SELECT * FROM `ProfessionalSkill` WHERE ProfessionalResumeId = @ProfessionalResumeId";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryAsync<ProfessionalSkill>(query, new { ProfessionalResumeId = professionalResumeId });
        }
    }

    /// <summary>
    /// Elimina todas las habilidades profesionales asociadas a un currículum profesional y crea nuevas habilidades profesionales en una sola transacción.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <param name="professionalSkills">Colección de habilidades profesionales a crear.</param>
    /// <returns>True si ambas operaciones fueron exitosas; de lo contrario, false.</returns>
    public async Task<bool> ReplaceProfessionalSkills(Guid professionalResumeId, IEnumerable<ProfessionalSkill> professionalSkills)
    {
        if (professionalSkills == null)
        {
            throw new ArgumentException("La lista de habilidades profesionales no puede ser nula.", nameof(professionalSkills));
        }

        string deleteQuery = "DELETE FROM `ProfessionalSkill` WHERE ProfessionalResumeId = @ProfessionalResumeId";
        string insertQuery = @"
            INSERT INTO `ProfessionalSkill` (
                ProfessionalResumeId, SkillId, Name
            ) VALUES (
                @ProfessionalResumeId, @SkillId, @Name
            );
        ";

        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // Eliminar todas las habilidades profesionales asociadas al ProfessionalResumeId
                    await connection.ExecuteAsync(deleteQuery, new { ProfessionalResumeId = professionalResumeId }, transaction);

                    // Crear nuevas habilidades profesionales
                    if (professionalSkills.Any())
                    {
                        int rowsAffected = await connection.ExecuteAsync(insertQuery, professionalSkills, transaction);

                        if (rowsAffected != professionalSkills.Count())
                        {
                            throw new Exception("No se pudieron crear todas las habilidades profesionales.");
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