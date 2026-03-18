using System;
using Dapper;
using EmployeeAPIviaDapper.Models;
using EmployeeAPIviaDapper.Repositories.Interface;
using Microsoft.Data.SqlClient;

namespace EmployeeAPIviaDapper.Repositories.Implementation;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly string _connectionString;
    public EmployeeRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }
    public async Task<IEnumerable<Employee>> GetEmployees()
    {
        using var connection = new SqlConnection(_connectionString);
        var employees = await connection.QueryAsync<Employee>("SELECT * FROM Employee");
        return employees;
    }
    public async Task<Employee?> GetEmployeeById(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        var employees = await connection.QueryFirstOrDefaultAsync<Employee>
         ("SELECT * FROM Employee WHERE Id = @id", new { id });
        return employees;
    }
    public async Task<int> CreateEmployee(Employee employee)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = "INSERT INTO Employee (Name, Department) VALUES (@Name, @Department)";
         var result = await connection.ExecuteAsync(sql, employee);
        return result;
    }
    public async Task<int> UpdateEmployee(int id, Employee employee)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = "UPDATE Employee SET Name = @Name, Department = @Department WHERE Id = @Id";
        var result = await connection.ExecuteAsync(sql, new { employee.Name, employee.Department, Id = id });
        return result;
    }
    public async Task<int> DeleteEmployee(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = "DELETE FROM Employee WHERE Id = @id";
        var result = await connection.ExecuteAsync(sql, new { id });
        return result;
    }
}
