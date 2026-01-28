using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace Resume.Infrastructure.DbContexts;

public class CompanyRecruitmentDbContext
{
    private readonly IConfiguration _configuration;

    public CompanyRecruitmentDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Proporciona una conexión abierta a la base de datos.
    /// </summary>
    /// <returns>Una conexión abierta a la base de datos.</returns>
    public async Task<IDbConnection> GetOpenConnectionAsync()
    {
        var connectionString = _configuration.GetConnectionString("CompanyRecruitmentConnection");
        var connection = new MySqlConnection(connectionString);

        await connection.OpenAsync(); // Abre la conexión antes de devolverla
        return connection;
    }
}
