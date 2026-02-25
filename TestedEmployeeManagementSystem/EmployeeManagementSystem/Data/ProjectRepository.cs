using Dapper;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Data;

public class ProjectRepository
{
    private readonly DbConnectionFactory _factory;

    public ProjectRepository(DbConnectionFactory factory)
    {
        _factory = factory;
    }

    public List<Project> GetAll()
    {
        using var conn = _factory.CreateConnection();
        var sql = @"
            SELECT Id, Name, StartDate, EndDate, Budget
            FROM Projects
            ORDER BY Id
        ";
        return conn.Query<Project>(sql).ToList();
    }
    public async Task<List<Project>> GetAllAsync()
    {
        using var conn = _factory.CreateConnection();
        var sql = @"
            SELECT Id, Name, StartDate, EndDate, Budget
            FROM Projects
            ORDER BY Id
        ";
        var result = await conn.QueryAsync<Project>(sql);
        return result.ToList();
    }

    public Project? GetById(int id)
    {
        using var conn = _factory.CreateConnection();
        var sql = @"
            SELECT Id, Name, StartDate, EndDate, Budget
            FROM Projects
            WHERE Id = @Id
        ";
        return conn.QuerySingleOrDefault<Project>(sql, new { Id = id });
    }
    public async Task<Project?> GetByIdAsync(int id)
    {
        using var conn = _factory.CreateConnection();
        var sql = @"
            SELECT Id, Name, StartDate, EndDate, Budget
            FROM Projects
            WHERE Id = @Id
        ";
        return await conn.QuerySingleOrDefaultAsync<Project>(sql, new { Id = id });
    }

    public Project Insert(Project project)
    {
        using var conn = _factory.CreateConnection();

        var sql = @"
            INSERT INTO Projects (Name, StartDate, EndDate, Budget)
            VALUES (@Name, @StartDate, @EndDate, @Budget);

            SELECT CAST(SCOPE_IDENTITY() AS int);
        ";

        var id = conn.ExecuteScalar<int>(sql, project);
        project.Id = id;
        return project;
    }
    public async Task<Project> InsertAsync(Project project)
    {
        using var conn = _factory.CreateConnection();
        var sql = @"
            INSERT INTO Projects (Name, StartDate, EndDate, Budget)
            VALUES (@Name, @StartDate, @EndDate, @Budget);
            SELECT CAST(SCOPE_IDENTITY() AS int);
        ";
        var id = await conn.ExecuteScalarAsync<int>(sql, project);
        project.Id = id;
        return project;
    }

    public Project Update(Project project)
    {
        using var conn = _factory.CreateConnection();

        var sql = @"
            UPDATE Projects
            SET
                Name = @Name,
                StartDate = @StartDate,
                EndDate = @EndDate,
                Budget = @Budget
            WHERE Id = @Id
        ";

        conn.Execute(sql, project);
        return project;
    }
    public async Task<Project> UpdateAsync(Project project)
    {
        using var conn = _factory.CreateConnection();
        var sql = @"
            UPDATE Projects
            SET
                Name = @Name,
                StartDate = @StartDate,
                EndDate = @EndDate,
                Budget = @Budget
            WHERE Id = @Id
        ";
        await conn.ExecuteAsync(sql, project);
        return project;
    }

    public void Delete(int id)
    {
        using var conn = _factory.CreateConnection();
        var sql = "DELETE FROM Projects WHERE Id = @Id";
        conn.Execute(sql, new { Id = id });
    }
    public async Task DeleteAsync(int id)
    {
        using var conn = _factory.CreateConnection();
        var sql = "DELETE FROM Projects WHERE Id = @Id";
        await conn.ExecuteAsync(sql, new { Id = id });
    }

    public bool ExistsByName(string name)
    {
        using var conn = _factory.CreateConnection();
        var sql = "SELECT COUNT(1) FROM Projects WHERE Name = @Name";
        return conn.ExecuteScalar<int>(sql, new { Name = name }) > 0;
    }
    public async Task<bool> ExistsByNameAsync(string name)
    {
        using var conn = _factory.CreateConnection();
        var sql = "SELECT COUNT(1) FROM Projects WHERE Name = @Name";
        return await conn.ExecuteScalarAsync<int>(sql, new { Name = name }) > 0;
    }
    public async Task<bool> ExistsByNameAsync(string name, int excludeId)
    {
        using var conn = _factory.CreateConnection();
        var sql = "SELECT COUNT(1) FROM Projects WHERE Name = @Name AND Id <> @ExcludeId";
        return await conn.ExecuteScalarAsync<int>(sql, new { Name = name, ExcludeId = excludeId }) > 0;
    }

    public bool ExistsByName(string name, int excludeId)
    {
        using var conn = _factory.CreateConnection();
        var sql = "SELECT COUNT(1) FROM Projects WHERE Name = @Name AND Id <> @ExcludeId";
        return conn.ExecuteScalar<int>(sql, new { Name = name, ExcludeId = excludeId }) > 0;
    }
}
