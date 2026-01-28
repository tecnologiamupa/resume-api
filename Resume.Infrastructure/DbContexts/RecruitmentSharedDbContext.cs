using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace Resume.Infrastructure.DbContexts;

public class RecruitmentSharedDbContext
{
    private readonly IConfiguration _configuration;

    public RecruitmentSharedDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IDbConnection> GetOpenConnectionAsync()
    {
        var connectionString = _configuration.GetConnectionString("RecruitmentSharedConnection");
        var connection = new MySqlConnection(connectionString);

        await connection.OpenAsync(); // Abre la conexión antes de devolverla
        return connection;
    }
}
