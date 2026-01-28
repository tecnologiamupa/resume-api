using Dapper;
using Resume.Core.Entities;
using Resume.Core.RepositoryContracts;
using Resume.Infrastructure.DbContexts;

namespace Resume.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de educación académica.
/// </summary>
internal class AcademicEducationRepository : IAcademicEducationRepository
{
    private readonly ResumeDbContext _dbContext;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="AcademicEducationRepository"/>.
    /// </summary>
    /// <param name="dbContext">Contexto de base de datos.</param>
    public AcademicEducationRepository(ResumeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Obtiene todas las entradas de educación académica asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <returns>Colección de entradas de educación académica.</returns>
    public async Task<IEnumerable<AcademicEducation>> GetAcademicEducationsByProfessionalResumeId(Guid professionalResumeId)
    {
        string query = "SELECT * FROM `AcademicEducation` WHERE ProfessionalResumeId = @ProfessionalResumeId";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryAsync<AcademicEducation>(query, new { ProfessionalResumeId = professionalResumeId });
        }
    }

    /// <summary>
    /// Reemplaza todas las entradas de educación académica asociadas a un currículum profesional.
    /// </summary>
    /// <param name="professionalResumeId">Identificador del currículum profesional.</param>
    /// <param name="academicEducations">Colección de entradas de educación académica a crear.</param>
    /// <returns>True si ambas operaciones fueron exitosas; de lo contrario, false.</returns>
    public async Task<bool> ReplaceAcademicEducations(Guid professionalResumeId, IEnumerable<AcademicEducation> academicEducations)
    {
        if (academicEducations == null)
        {
            throw new ArgumentException("La lista de entradas de educación académica no puede ser nula.", nameof(academicEducations));
        }

        string deleteQuery = "DELETE FROM `AcademicEducation` WHERE ProfessionalResumeId = @ProfessionalResumeId";
        string insertQuery = @"
        INSERT INTO `AcademicEducation` (
            ProfessionalResumeId, Institution, Degree, FieldOfStudy, StartDate, EndDate, CurrentlyStudying, AdditionalDescription, CreatedDate, CreatedBy
        ) VALUES (
            @ProfessionalResumeId, @Institution, @Degree, @FieldOfStudy, @StartDate, @EndDate, @CurrentlyStudying, @AdditionalDescription, @CreatedDate, @CreatedBy
        );
    ";

        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // Eliminar todas las entradas de educación académica asociadas al ProfessionalResumeId
                    await connection.ExecuteAsync(deleteQuery, new { ProfessionalResumeId = professionalResumeId }, transaction);

                    // Crear nuevas entradas de educación académica
                    if (academicEducations.Any())
                    {
                        int rowsAffected = await connection.ExecuteAsync(insertQuery, academicEducations, transaction);

                        if (rowsAffected != academicEducations.Count())
                        {
                            throw new Exception("No se pudieron crear todas las entradas de educación académica.");
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