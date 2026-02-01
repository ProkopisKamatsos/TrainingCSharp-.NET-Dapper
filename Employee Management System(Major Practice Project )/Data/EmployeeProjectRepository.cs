using Dapper;

namespace EmployeeManagementSystem.Data;

public class EmployeeProjectRepository
{
    private readonly DbConnectionFactory _factory;

    public EmployeeProjectRepository(DbConnectionFactory factory)
    {
        _factory = factory;
    }

    public void Assign(int employeeId, int projectId, string role)
    {
        using var conn = _factory.CreateConnection();

        var sql = @"
        INSERT INTO EmployeeProjects (EmployeeId, ProjectId, Role)
        VALUES (@EmployeeId, @ProjectId, @Role);
    ";

        conn.Execute(sql, new { EmployeeId = employeeId, ProjectId = projectId, Role = role });
    }
    public async Task AssignAsync(int employeeId, int projectId, string role)
    {
        using var conn = _factory.CreateConnection();
        var sql = @"
        INSERT INTO EmployeeProjects (EmployeeId, ProjectId, Role)
        VALUES (@EmployeeId, @ProjectId, @Role);";
        await conn.ExecuteAsync(sql, new { EmployeeId = employeeId, ProjectId = projectId, Role = role });

    }
}