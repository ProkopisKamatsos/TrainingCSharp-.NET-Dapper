using System;
using EmployeeAPIviaDapper.Models;

namespace EmployeeAPIviaDapper.Services.Interface;

public interface IEmployeeService
{
    public Task<IEnumerable<Employee>> GetEmployees();
    public Task<Employee?> GetEmployeeById(int id);
    public Task<int> CreateEmployee(Employee employee);
    public Task<int> UpdateEmployee(int id, Employee employee);
    public Task<int> DeleteEmployee(int id);
}
