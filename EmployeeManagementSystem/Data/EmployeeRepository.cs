using System.Data;
using Dapper;
using EmployeeManagementSystem.Models;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Data;

public class EmployeeRepository
{
    private readonly DbConnectionFactory _factory;

    public EmployeeRepository(DbConnectionFactory factory)
    {
        _factory = factory;
    }

    public Employee? GetById(int id)
    {
        using var conn = _factory.CreateConnection();

        var sql = @"
        SELECT 
            Id, FirstName, LastName, Email, Phone,
            DepartmentId, Salary, HireDate, IsActive
        FROM Employees
        WHERE Id = @Id
    ";

        return conn.QuerySingleOrDefault<Employee>(sql, new { Id = id });
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        using var conn = _factory.CreateConnection();

        var sql = @"
        SELECT 
            Id, FirstName, LastName, Email, Phone,
            DepartmentId, Salary, HireDate, IsActive
        FROM Employees
        WHERE Id = @Id
    ";

        return await conn.QuerySingleOrDefaultAsync<Employee>(sql, new { Id = id });
    }


    public List<Employee> GetAll()
    {
        using var conn = _factory.CreateConnection();

        var sql = @"
        SELECT 
            Id, FirstName, LastName, Email, Phone,
            DepartmentId, Salary, HireDate, IsActive
        FROM Employees
        ORDER BY Id
    ";

        return conn.Query<Employee>(sql).ToList();
    }
    public async Task<List<Employee>> GetAllAsync()
    {
        using var conn = _factory.CreateConnection();
        var sql = @"SELECT Id, FirstName, LastName, Email, Phone, DepartmentId, Salary, HireDate, IsActive
                FROM Employees ORDER BY Id";
        var rows = await conn.QueryAsync<Employee>(sql);
        return rows.ToList();
    }


    public Employee Insert(Employee employee)
    {
        using var conn = _factory.CreateConnection();

        var sql = @"
        INSERT INTO Employees
        (
            FirstName,
            LastName,
            Email,
            Phone,
            DepartmentId,
            Salary,
            HireDate,
            IsActive
        )
        VALUES
        (
            @FirstName,
            @LastName,
            @Email,
            @Phone,
            @DepartmentId,
            @Salary,
            @HireDate,
            @IsActive
        );

        SELECT CAST(SCOPE_IDENTITY() AS int);
    ";

        var id = conn.ExecuteScalar<int>(sql, employee);

        employee.Id = id;
        return employee;
    }
    public async Task<Employee> InsertAsync(Employee employee)
    {
        using var conn = _factory.CreateConnection();
        var sql = @"
        INSERT INTO Employees (FirstName, LastName, Email, Phone, DepartmentId, Salary, HireDate, IsActive)
        VALUES (@FirstName, @LastName, @Email, @Phone, @DepartmentId, @Salary, @HireDate, @IsActive);
        SELECT CAST(SCOPE_IDENTITY() AS int);";

        var id = await conn.ExecuteScalarAsync<int>(sql, employee);
        employee.Id = id;
        return employee;
    }
    public Employee Update(Employee employee)
    {
        using var conn = _factory.CreateConnection();

        var sql = @"
        UPDATE Employees
        SET
            FirstName = @FirstName,
            LastName = @LastName,
            Email = @Email,
            Phone = @Phone,
            DepartmentId = @DepartmentId,
            Salary = @Salary,
            IsActive = @IsActive
        WHERE Id = @Id
    ";

        conn.Execute(sql, employee);
        return employee;
    }
    public async Task<Employee> UpdateAsync(Employee employee)
    {
        using var conn = _factory.CreateConnection();
        var sql = @"
        UPDATE Employees SET FirstName=@FirstName, LastName=@LastName, Email=@Email, Phone=@Phone,
            DepartmentId=@DepartmentId, Salary=@Salary, IsActive=@IsActive
        WHERE Id=@Id";
        await conn.ExecuteAsync(sql, employee);
        return employee;
    }

    public void SoftDelete(int id)
    {
        using var conn = _factory.CreateConnection();

        var sql = @"
        UPDATE Employees
        SET IsActive = 0
        WHERE Id = @Id
    ";

        conn.Execute(sql, new { Id = id });
    }
    public async Task SoftDeleteAsync(int id)
    {
        using var conn = _factory.CreateConnection();
        var sql = @"UPDATE Employees SET IsActive=0 WHERE Id=@Id";
        await conn.ExecuteAsync(sql, new { Id = id });
    }
    public bool AnyInDepartment(int departmentId)
    {
        using var conn = _factory.CreateConnection();
        var sql = "SELECT COUNT(1) FROM Employees WHERE DepartmentId = @DepartmentId AND IsActive = 1";
        return conn.ExecuteScalar<int>(sql, new { DepartmentId = departmentId }) > 0;
    }
    public async Task<bool> AnyInDepartmentAsync(int departmentId)
    {
        using var conn = _factory.CreateConnection();
        var sql = "SELECT COUNT(1) FROM Employees WHERE DepartmentId=@DepartmentId AND IsActive=1";
        return await conn.ExecuteScalarAsync<int>(sql, new { DepartmentId = departmentId }) > 0;
    }
    public List<Employee> Search(EmployeeSearchCriteria criteria)
    {
        using var conn = _factory.CreateConnection();

        var sql = @"
        SELECT Id, FirstName, LastName, Email, Phone, DepartmentId, Salary, HireDate, IsActive
        FROM Employees
        WHERE IsActive = 1
    ";

        var conditions = new List<string>();
        var parameters = new DynamicParameters();


        if (!string.IsNullOrWhiteSpace(criteria.Name))
        {
            conditions.Add("(FirstName LIKE @Name OR LastName LIKE @Name)");
            parameters.Add("Name", $"%{criteria.Name.Trim()}%");
        }

        if (criteria.DepartmentId.HasValue)
        {
            conditions.Add("DepartmentId = @DepartmentId");
            parameters.Add("DepartmentId", criteria.DepartmentId.Value);
        }


        if (criteria.SalaryFrom.HasValue && criteria.SalaryTo.HasValue)
        {
            conditions.Add("Salary BETWEEN @SalaryFrom AND @SalaryTo");
            parameters.Add("SalaryFrom", criteria.SalaryFrom.Value);
            parameters.Add("SalaryTo", criteria.SalaryTo.Value);
        }


        if (conditions.Count > 0)
        {
            sql += " AND " + string.Join(" AND ", conditions);
        }

        sql += " ORDER BY Id";

        return conn.Query<Employee>(sql, parameters).ToList();
    }
    public async Task<List<Employee>> SearchAsync(EmployeeSearchCriteria criteria)
    {
        using var conn = _factory.CreateConnection();

        var sql = @"SELECT Id, FirstName, LastName, Email, Phone, DepartmentId, Salary, HireDate, IsActive
                FROM Employees WHERE IsActive=1";

        var conditions = new List<string>();
        var parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(criteria.Name))
        {
            conditions.Add("(FirstName LIKE @Name OR LastName LIKE @Name)");
            parameters.Add("Name", $"%{criteria.Name.Trim()}%");
        }
        if (criteria.DepartmentId.HasValue)
        {
            conditions.Add("DepartmentId = @DepartmentId");
            parameters.Add("DepartmentId", criteria.DepartmentId.Value);
        }
        if (criteria.SalaryFrom.HasValue && criteria.SalaryTo.HasValue)
        {
            conditions.Add("Salary BETWEEN @SalaryFrom AND @SalaryTo");
            parameters.Add("SalaryFrom", criteria.SalaryFrom.Value);
            parameters.Add("SalaryTo", criteria.SalaryTo.Value);
        }

        if (conditions.Count > 0)
            sql += " AND " + string.Join(" AND ", conditions);

        sql += " ORDER BY Id";

        var rows = await conn.QueryAsync<Employee>(sql, parameters);
        return rows.ToList();
    }
    public Employee? GetEmployeeWithDepartmentById(int employeeId)
    {
        using var conn = _factory.CreateConnection();

        var sql = @"
        SELECT 
            e.Id, e.FirstName, e.LastName, e.Email, e.Phone,
            e.DepartmentId, e.Salary, e.HireDate, e.IsActive,
            d.Id, d.Name, d.Location, d.ManagerId
        FROM Employees e
        INNER JOIN Departments d ON e.DepartmentId = d.Id
        WHERE e.Id = @EmployeeId AND e.IsActive = 1
    ";

        return conn.Query<Employee, Department, Employee>(
            sql,
            (emp, dept) =>
            {
                emp.Department = dept;
                return emp;
            },
            new { EmployeeId = employeeId },
            splitOn: "Id"
        ).SingleOrDefault();
    }
    public async Task<Employee?> GetEmployeeWithDepartmentByIdAsync(int employeeId)
    {
        using var conn = _factory.CreateConnection();

        var sql = @"
        SELECT e.Id, e.FirstName, e.LastName, e.Email, e.Phone, e.DepartmentId, e.Salary, e.HireDate, e.IsActive,
               d.Id, d.Name, d.Location, d.ManagerId
        FROM Employees e
        INNER JOIN Departments d ON e.DepartmentId = d.Id
        WHERE e.Id = @EmployeeId AND e.IsActive = 1";

        var rows = await conn.QueryAsync<Employee, Department, Employee>(
            sql,
            (emp, dept) => { emp.Department = dept; return emp; },
            new { EmployeeId = employeeId },
            splitOn: "Id"
        );

        return rows.SingleOrDefault();
    }
    public Employee? GetEmployeeWithProjectsById(int employeeId)
    {
        using var conn = _factory.CreateConnection();

        var sql = @"
        SELECT
            e.Id, e.FirstName, e.LastName, e.Email, e.Phone,
            e.DepartmentId, e.Salary, e.HireDate, e.IsActive,

            ep.EmployeeId, ep.ProjectId, ep.Role,

            p.Id, p.Name, p.StartDate, p.EndDate, p.Budget
        FROM Employees e
        LEFT JOIN EmployeeProjects ep ON ep.EmployeeId = e.Id
        LEFT JOIN Projects p ON p.Id = ep.ProjectId
        WHERE e.Id = @EmployeeId
        ORDER BY p.Id;
    ";

        var dict = new Dictionary<int, Employee>();

        conn.Query<Employee, EmployeeProject, Project, Employee>(
            sql,
            (emp, ep, proj) =>
            {
                if (!dict.TryGetValue(emp.Id, out var existing))
                {
                    existing = emp;
                    existing.ProjectAssignments = new List<ProjectAssignment>();
                    dict.Add(existing.Id, existing);
                }

                if (proj != null && proj.Id != 0)
                {
                    existing.ProjectAssignments.Add(new ProjectAssignment
                    {
                        Project = proj,
                        Role = ep?.Role ?? ""
                    });
                }

                return existing;
            },
            new { EmployeeId = employeeId },
            splitOn: "EmployeeId,Id"
        );

        return dict.Values.SingleOrDefault();
    }
    public async Task<Employee?> GetEmployeeWithProjectsByIdAsync(int employeeId)
    {
        using var conn = _factory.CreateConnection();

        var sql = @"
        SELECT e.Id, e.FirstName, e.LastName, e.Email, e.Phone, e.DepartmentId, e.Salary, e.HireDate, e.IsActive,
               ep.EmployeeId, ep.ProjectId, ep.Role,
               p.Id, p.Name, p.StartDate, p.EndDate, p.Budget
        FROM Employees e
        LEFT JOIN EmployeeProjects ep ON ep.EmployeeId = e.Id
        LEFT JOIN Projects p ON p.Id = ep.ProjectId
        WHERE e.Id = @EmployeeId
        ORDER BY p.Id;";

        var dict = new Dictionary<int, Employee>();

        await conn.QueryAsync<Employee, EmployeeProject, Project, Employee>(
            sql,
            (emp, ep, proj) =>
            {
                if (!dict.TryGetValue(emp.Id, out var existing))
                {
                    existing = emp;
                    existing.ProjectAssignments = new List<ProjectAssignment>();
                    dict.Add(existing.Id, existing);
                }

                if (proj != null && proj.Id != 0)
                {
                    existing.ProjectAssignments.Add(new ProjectAssignment
                    {
                        Project = proj,
                        Role = ep?.Role ?? ""
                    });
                }

                return existing;
            },
            new { EmployeeId = employeeId },
            splitOn: "EmployeeId,Id"
        );

        return dict.Values.SingleOrDefault();
    }

    public void UpdateSalaryWithHistory(int employeeId, decimal newSalary)
    {
        using var conn = _factory.CreateConnection();
        conn.Open();

        using var tx = conn.BeginTransaction();

        try
        {
            var oldSalary = conn.QueryFirstOrDefault<decimal?>(
                "SELECT Salary FROM Employees WHERE Id = @Id",
                new { Id = employeeId },
                transaction: tx
            );

            if (oldSalary == null)
                throw new Exception("Employee not found.");

            var rows = conn.Execute(
                "UPDATE Employees SET Salary = @Salary WHERE Id = @Id",
                new { Salary = newSalary, Id = employeeId },
                transaction: tx
            );

            if (rows != 1)
                throw new Exception("Salary update failed.");

            conn.Execute(
                @"INSERT INTO EmployeeSalaryHistory (EmployeeId, OldSalary, NewSalary)
              VALUES (@EmployeeId, @OldSalary, @NewSalary)",
                new { EmployeeId = employeeId, OldSalary = oldSalary.Value, NewSalary = newSalary },
                transaction: tx
            );

            tx.Commit();
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }
    public async Task UpdateSalaryWithHistoryAsync(int employeeId, decimal newSalary)
    {
        using var conn = _factory.CreateConnection();
        conn.Open();

        using var tx = conn.BeginTransaction();

        try
        {
            var oldSalary = await conn.QueryFirstOrDefaultAsync<decimal?>(
                "SELECT Salary FROM Employees WHERE Id = @Id",
                new { Id = employeeId },
                transaction: tx);

            if (oldSalary == null)
                throw new Exception("Employee not found.");

            var rows = await conn.ExecuteAsync(
                "UPDATE Employees SET Salary = @Salary WHERE Id = @Id",
                new { Salary = newSalary, Id = employeeId },
                transaction: tx);

            if (rows != 1)
                throw new Exception("Salary update failed.");

            await conn.ExecuteAsync(
                @"INSERT INTO EmployeeSalaryHistory (EmployeeId, OldSalary, NewSalary)
              VALUES (@EmployeeId, @OldSalary, @NewSalary)",
                new { EmployeeId = employeeId, OldSalary = oldSalary.Value, NewSalary = newSalary },
                transaction: tx);

            tx.Commit();
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }


}
