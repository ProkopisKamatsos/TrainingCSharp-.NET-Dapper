using System;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Data
{
    public interface IDepartmentRepository
    {
        bool Exists(int departmentId);
        Task<bool> ExistsAsync(int departmentId);
        Department? GetById(int id);
        Task<Department?> GetByIdAsync(int id);
        bool ExistsByName(string name, int excludeId);
        Task<bool> ExistsByNameAsync(string name);
        void Delete(int id);
        Task DeleteAsync(int id);
        bool ExistsByName(string name);
        Task<bool> ExistsByNameAsync(string name, int excludeId);
        List<Department> GetAll();
        Task<List<Department>> GetAllAsync();
        Department Update(Department department);
        Task<Department> UpdateAsync(Department department);
        Department Insert(Department department);
        Task<Department> InsertAsync(Department department);
        DepartmentTotals? GetDepartmentTotalsById(int departmentId);
        Task<DepartmentTotals?> GetDepartmentTotalsByIdAsync(int departmentId);
        DepartmentTotals? GetDepartmentTotalsById_SP(int departmentId);
        Task<DepartmentTotals?> GetDepartmentTotalsById_SPAsync(int departmentId);
    }
}