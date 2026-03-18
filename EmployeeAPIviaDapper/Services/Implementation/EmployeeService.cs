using System;
using EmployeeAPIviaDapper.Models;
using EmployeeAPIviaDapper.Repositories.Interface;
using EmployeeAPIviaDapper.Services.Interface;

namespace EmployeeAPIviaDapper.Services.Implementation;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repo;
    public EmployeeService(IEmployeeRepository repo)
    {
        _repo = repo;
    }
    public async Task<IEnumerable<Employee>> GetEmployees()
    {
        return await _repo.GetEmployees();
    }
    public async Task<Employee?> GetEmployeeById(int id)
    {
        return await _repo.GetEmployeeById(id);
    }
    public async Task<int> CreateEmployee(Employee employee)
    {
        return await _repo.CreateEmployee(employee);
    }
    public async Task<int> UpdateEmployee(int id, Employee employee)
    {
        return await _repo.UpdateEmployee(id, employee);
    }
    public async Task<int> DeleteEmployee(int id)
    {
        return await _repo.DeleteEmployee(id);
    }
}
