using System.Data;

namespace TaskManagementAPI.Data;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
