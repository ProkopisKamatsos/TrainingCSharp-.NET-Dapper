using Dapper;
using EmployeeManagementSystem.Models;
using System.Data;

namespace EmployeeManagementSystem.Data;

public class DepartmentRepository
{
    private readonly DbConnectionFactory _factory;

    public DepartmentRepository(DbConnectionFactory factory)
    {
        _factory = factory;
    }

    public bool Exists(int departmentId)
    {
        using var conn = _factory.CreateConnection();
        var sql = "SELECT COUNT(1) FROM Departments WHERE Id = @Id";
        return conn.ExecuteScalar<int>(sql, new { Id = departmentId }) > 0;
    }
    public async Task<bool> ExistsAsync(int departmentId)
    {
        using var conn = _factory.CreateConnection();
        var sql = "SELECT COUNT(1) FROM Departments WHERE Id = @Id";
        return await conn.ExecuteScalarAsync<int>(sql, new { Id = departmentId }) > 0;
    }

    public Department? GetById(int id)
    {
        using var conn = _factory.CreateConnection();
        var sql = @"
        SELECT Id, Name, Location, ManagerId
        FROM Departments
        WHERE Id = @Id
    ";
        return conn.QuerySingleOrDefault<Department>(sql, new { Id = id });
    }
    public async Task<Department?> GetByIdAsync(int id)
    {
        using var conn = _factory.CreateConnection();
        var sql = @"
        SELECT Id, Name, Location, ManagerId
        FROM Departments
        WHERE Id = @Id"
;
        return await conn.QuerySingleOrDefaultAsync<Department>(sql, new { Id = id });
    }

    public bool ExistsByName(string name, int excludeId)
    {
        using var conn = _factory.CreateConnection();
        var sql = "SELECT COUNT(1) FROM Departments WHERE Name = @Name AND Id <> @ExcludeId";
        return conn.ExecuteScalar<int>(sql, new { Name = name, ExcludeId = excludeId }) > 0;
    }
    public async Task<bool> ExistsByNameAsync(string name)
    {
        using var conn = _factory.CreateConnection();
        var sql = "SELECT COUNT(1) FROM Departments WHERE Name = @Name";
        return await conn.ExecuteScalarAsync<int>(sql, new { Name = name }) > 0;
    }

    public void Delete(int id)
    {
        using var conn = _factory.CreateConnection();
        var sql = "DELETE FROM Departments WHERE Id = @Id";
        conn.Execute(sql, new { Id = id });
    }
    public async Task DeleteAsync(int id)
    {
        using var conn = _factory.CreateConnection();
        var sql = "DELETE FROM Departments WHERE Id = @Id";
        await conn.ExecuteAsync(sql, new { Id = id });
    }
    public bool ExistsByName(string name)
    {
        using var conn = _factory.CreateConnection();
        var sql = "SELECT COUNT(1) FROM Departments WHERE Name = @Name";
        return conn.ExecuteScalar<int>(sql, new { Name = name }) > 0;
    }
    public async Task<bool> ExistsByNameAsync(string name, int excludeId)
    {
        using var conn = _factory.CreateConnection();
        var sql = "SELECT COUNT(1) FROM Departments WHERE Name = @Name AND Id <> @ExcludeId";
        return await conn.ExecuteScalarAsync<int>(sql, new { Name = name, ExcludeId = excludeId }) > 0;
    }
    public List<Department> GetAll()
    {
        using var conn = _factory.CreateConnection();

        var sql = @"
        SELECT Id, Name, Location, ManagerId
        FROM Departments
        ORDER BY Name
    ";

        return conn.Query<Department>(sql).ToList();
    }
    public async Task<List<Department>> GetAllAsync()
    {
        using var conn = _factory.CreateConnection();
        var sql = @"
        SELECT Id, Name, Location, ManagerId
        FROM Departments
        ORDER BY Name";
        var result = await conn.QueryAsync<Department>(sql);
        return result.ToList();
    }


    public Department Update(Department department)
    {
        using var conn = _factory.CreateConnection();

        var sql = @"
        UPDATE Departments
        SET
            Name = @Name,
            Location = @Location,
            ManagerId = @ManagerId
        WHERE Id = @Id
    ";

        conn.Execute(sql, department);
        return department;
    }
    public async Task<Department> UpdateAsync(Department department)
    {
        using var conn = _factory.CreateConnection();
        var sql = @"
        UPDATE Departments
        SET
            Name = @Name,
            Location = @Location,
            ManagerId = @ManagerId
        WHERE Id = @Id";
        await conn.ExecuteAsync(sql, department);
        return department;
    }

    public Department Insert(Department department)
    {
        using var conn = _factory.CreateConnection();

        var sql = @"
        INSERT INTO Departments (Name, Location, ManagerId)
        VALUES (@Name, @Location, @ManagerId);

        SELECT CAST(SCOPE_IDENTITY() AS int);
    ";

        var id = conn.ExecuteScalar<int>(sql, department);
        department.Id = id;
        return department;
    }
    public async Task<Department> InsertAsync(Department department)
    {
        using var conn = _factory.CreateConnection();
        var sql = @"
        INSERT INTO Departments (Name, Location, ManagerId)
        VALUES (@Name, @Location, @ManagerId);
        SELECT CAST(SCOPE_IDENTITY() AS int);";
        var id = await conn.ExecuteScalarAsync<int>(sql, department);
        department.Id = id;
        return department;
    }
    public DepartmentTotals? GetDepartmentTotalsById(int departmentId)
    {
        using var conn = _factory.CreateConnection();

        var sql = @"
        SELECT
            d.Id AS DepartmentId,
            d.Name AS DepartmentName,

            COUNT(e.Id) AS TotalEmployeeCount,
            SUM(CASE WHEN e.IsActive = 1 THEN 1 ELSE 0 END) AS ActiveEmployeeCount,
            SUM(CASE WHEN e.IsActive = 0 THEN 1 ELSE 0 END) AS InactiveEmployeeCount,

            COALESCE(SUM(CASE WHEN e.IsActive = 1 THEN e.Salary ELSE 0 END), 0) AS ActiveTotalSalary,
            COALESCE(AVG(CASE WHEN e.IsActive = 1 THEN e.Salary END), 0) AS ActiveAverageSalary
        FROM Departments d
        LEFT JOIN Employees e
            ON e.DepartmentId = d.Id
        WHERE d.Id = @departmentId
        GROUP BY d.Id, d.Name;
    ";

        return conn.QueryFirstOrDefault<DepartmentTotals>(sql, new { departmentId });
    }

    public async Task<DepartmentTotals?> GetDepartmentTotalsByIdAsync(int departmentId)
    {
        using var conn = _factory.CreateConnection();
        var sql = @"
        SELECT
            d.Id AS DepartmentId,
            d.Name AS DepartmentName,
            COUNT(e.Id) AS TotalEmployeeCount,
            SUM(CASE WHEN e.IsActive = 1 THEN 1 ELSE 0 END) AS ActiveEmployeeCount,
            SUM(CASE WHEN e.IsActive = 0 THEN 1 ELSE 0 END) AS InactiveEmployeeCount,
            COALESCE(SUM(CASE WHEN e.IsActive = 1 THEN e.Salary ELSE 0 END), 0) AS ActiveTotalSalary,
            COALESCE(AVG(CASE WHEN e.IsActive = 1 THEN e.Salary END), 0) AS ActiveAverageSalary
        FROM Departments d
        LEFT JOIN Employees e
            ON e.DepartmentId = d.Id
        WHERE d.Id = @departmentId
        GROUP BY d.Id, d.Name;";
        return await conn.QueryFirstOrDefaultAsync<DepartmentTotals>(sql, new { departmentId });
    }

    public DepartmentTotals? GetDepartmentTotalsById_SP(int departmentId)
    {
        using var conn = _factory.CreateConnection();

        return conn.QueryFirstOrDefault<DepartmentTotals>(
            "dbo.sp_DepartmentTotalsById",
            new { DepartmentId = departmentId },
            commandType: CommandType.StoredProcedure
        );
    }
    public async Task<DepartmentTotals?> GetDepartmentTotalsById_SPAsync(int departmentId)
    {
        using var conn = _factory.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<DepartmentTotals>(
            "dbo.sp_DepartmentTotalsById",
            new { DepartmentId = departmentId },
            commandType: CommandType.StoredProcedure
        );
    }


}
