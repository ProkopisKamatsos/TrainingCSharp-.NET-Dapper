using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Data
{
    public interface IEmployeeRepository
    {
        // READ
        Employee? GetById(int id);
        Task<Employee?> GetByIdAsync(int id);

        List<Employee> GetAll();
        Task<List<Employee>> GetAllAsync();

        // CREATE
        Employee Insert(Employee employee);
        Task<Employee> InsertAsync(Employee employee);

        // UPDATE
        Employee Update(Employee employee);
        Task<Employee> UpdateAsync(Employee employee);

        // DELETE (soft)
        void SoftDelete(int id);
        Task SoftDeleteAsync(int id);

        // CHECKS / QUERIES
        bool AnyInDepartment(int departmentId);
        Task<bool> AnyInDepartmentAsync(int departmentId);

        List<Employee> Search(EmployeeSearchCriteria criteria);
        Task<List<Employee>> SearchAsync(EmployeeSearchCriteria criteria);

        // JOINS / MULTI-MAPPING
        Employee? GetEmployeeWithDepartmentById(int employeeId);
        Task<Employee?> GetEmployeeWithDepartmentByIdAsync(int employeeId);

        Employee? GetEmployeeWithProjectsById(int employeeId);
        Task<Employee?> GetEmployeeWithProjectsByIdAsync(int employeeId);

        // TRANSACTIONS
        void UpdateSalaryWithHistory(int employeeId, decimal newSalary);
        Task UpdateSalaryWithHistoryAsync(int employeeId, decimal newSalary);

        // BULK
        Task BulkInsertAsync(List<Employee> employees);
    }
}