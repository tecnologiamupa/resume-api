using Dapper;
using Resume.Core.Entities;
using Resume.Core.RepositoryContracts;
using Resume.Infrastructure.DbContexts;

namespace Resume.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de idiomas personales.
/// </summary>
internal class LanguageRepository : ILanguageRepository
{
    private readonly ResumeDbContext _dbContext;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="LanguageRepository"/>.
    /// </summary>
    /// <param name="dbContext">Contexto de base de datos.</param>
    public LanguageRepository(ResumeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Obtiene todos los idiomas asociados a una información personal.
    /// </summary>
    /// <param name="personalInfoId">Identificador de la información personal.</param>
    /// <returns>Colección de idiomas personales.</returns>
    public async Task<IEnumerable<PersonalLanguage>> GetLanguagesByPersonalInfoId(Guid personalInfoId)
    {
        string query = "SELECT * FROM `PersonalLanguage` WHERE PersonalInfoId = @PersonalInfoId";
        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            return await connection.QueryAsync<PersonalLanguage>(query, new { PersonalInfoId = personalInfoId });
        }
    }

    /// <summary>
    /// Elimina todos los idiomas asociados a una información personal y crea nuevos idiomas en una sola transacción.
    /// </summary>
    /// <param name="personalInfoId">Identificador de la información personal.</param>
    /// <param name="languages">Colección de idiomas a crear.</param>
    /// <returns>True si ambas operaciones fueron exitosas; de lo contrario, false.</returns>
    public async Task<bool> ReplaceLanguages(Guid personalInfoId, IEnumerable<PersonalLanguage> languages)
    {
        if (languages == null)
        {
            throw new ArgumentException("La lista de idiomas no puede ser nula.", nameof(languages));
        }

        string deleteQuery = "DELETE FROM `PersonalLanguage` WHERE PersonalInfoId = @PersonalInfoId";
        string insertQuery = @"
        INSERT INTO `PersonalLanguage` (
            PersonalInfoId, LanguageId, Name
        ) VALUES (
            @PersonalInfoId, @LanguageId, @Name
        );
    ";

        using (var connection = await _dbContext.GetOpenConnectionAsync())
        {
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // Eliminar todos los idiomas asociados al PersonalInfoId
                    await connection.ExecuteAsync(deleteQuery, new { PersonalInfoId = personalInfoId }, transaction);

                    // Crear nuevos idiomas
                    if (languages.Any())
                    {
                        int rowsAffected = await connection.ExecuteAsync(insertQuery, languages, transaction);

                        if (rowsAffected != languages.Count())
                        {
                            throw new Exception("No se pudieron crear todos los idiomas.");
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