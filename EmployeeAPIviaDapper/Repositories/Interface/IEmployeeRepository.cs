using System;
using EmployeeAPIviaDapper.Models;

namespace EmployeeAPIviaDapper.Repositories.Interface;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetEmployees();
    Task<Employee?> GetEmployeeById(int id);
    Task<int> CreateEmployee(Employee employee);
    Task<int> UpdateEmployee(int id, Employee employee);
    Task<int> DeleteEmployee(int id);
}
