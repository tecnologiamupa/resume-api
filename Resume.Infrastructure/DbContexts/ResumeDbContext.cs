using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace Resume.Infrastructure.DbContexts;

/// <summary>
/// Proporciona un contexto de base de datos utilizando Dapper y MySQL.
/// </summary>
public class ResumeDbContext
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ResumeDbContext"/>.
    /// </summary>
    /// <param name="configuration">La configuración de la aplicación que contiene la cadena de conexión.</param>
    public ResumeDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Proporciona una conexión abierta a la base de datos.
    /// </summary>
    /// <returns>Una conexión abierta a la base de datos.</returns>
    public async Task<IDbConnection> GetOpenConnectionAsync()
    {
        var connectionString = _configuration.GetConnectionString("ResumeConnection");
        var connection = new MySqlConnection(connectionString);

        await connection.OpenAsync(); // Abre la conexión antes de devolverla
        return connection;
    }
}
