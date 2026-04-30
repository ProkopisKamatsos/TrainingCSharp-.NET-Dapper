using Microsoft.Data.SqlClient;

namespace BankingApi.Data;

public class SqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string not found.");
    }

    public SqlConnection Create()
    {
        return new SqlConnection(_connectionString);
    }
}