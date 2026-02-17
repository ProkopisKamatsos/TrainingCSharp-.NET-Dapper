using Microsoft.Data.SqlClient;
using System.Data;

namespace WebApplication1.Data
{
    public class DbContext
    {
        private readonly string _connectionString;
        public DbContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
            
        }
        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
       

    }
}
