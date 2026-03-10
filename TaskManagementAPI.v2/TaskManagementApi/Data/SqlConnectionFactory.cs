using System.Data;
using Microsoft.Data.SqlClient;

namespace TaskManagementApi.Data;

public class SqlConnectionFactory
{
    private readonly IConfiguration _configuration;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection CreateConnection()
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("DefaultConnection is missing from configuration.");
        }

        return new SqlConnection(connectionString);
    }
}