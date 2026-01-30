using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Services;

public class DepartmentService
{
    private readonly DepartmentRepository _departmentRepo;
    private readonly EmployeeRepository _employeeRepo;

    public DepartmentService(DepartmentRepository departmentRepo, EmployeeRepository employeeRepo)
    {
        _departmentRepo = departmentRepo;
        _employeeRepo = employeeRepo;
    }
    public List<Department> GetAllDepartments()
    {
        return _departmentRepo.GetAll();
    }
    public async Task<List<Department>> GetAllDepartmentsAsync()
    {
        return await _departmentRepo.GetAllAsync();
    }

    public Department CreateDepartment(Department department)
    {
        if (string.IsNullOrWhiteSpace(department.Name))
            throw new Exception("Department name is required");

        department.Name = department.Name.Trim();

        if (_departmentRepo.ExistsByName(department.Name))
            throw new Exception("Department name already exists");

        department.Location ??= "";

        return _departmentRepo.Insert(department);
    }
    public async Task<Department> CreateDepartmentAsync(Department department)
    {
        if (string.IsNullOrWhiteSpace(department.Name))
            throw new Exception("Department name is required");
        department.Name = department.Name.Trim();
        if (await _departmentRepo.ExistsByNameAsync(department.Name))
            throw new Exception("Department name already exists");
        department.Location ??= "";
        return await _departmentRepo.InsertAsync(department);
    }
    public Department UpdateDepartment(Department department)
    {
        if (department.Id <= 0)
            throw new Exception("Invalid department Id");

        var existing = _departmentRepo.GetById(department.Id);
        if (existing is null)
            throw new Exception("Department not found");

        if (string.IsNullOrWhiteSpace(department.Name))
            throw new Exception("Department name is required");

        department.Name = department.Name.Trim();
        department.Location ??= "";

        if (_departmentRepo.ExistsByName(department.Name, department.Id))
            throw new Exception("Department name already exists");

        return _departmentRepo.Update(department);
    }
    public async Task<Department> UpdateDepartmentAsync(Department department)
    {
        if (department.Id <= 0)
            throw new Exception("Invalid department Id");
        var existing = await _departmentRepo.GetByIdAsync(department.Id);
        if (existing is null)
            throw new Exception("Department not found");
        if (string.IsNullOrWhiteSpace(department.Name))
            throw new Exception("Department name is required");
        department.Name = department.Name.Trim();
        department.Location ??= "";
        if (await _departmentRepo.ExistsByNameAsync(department.Name, department.Id))
            throw new Exception("Department name already exists");
        return await _departmentRepo.UpdateAsync(department);
    }
    public void DeleteDepartment(int id)
    {
        if (id <= 0)
            throw new Exception("Invalid department Id");

        var dept = _departmentRepo.GetById(id);
        if (dept is null)
            throw new Exception("Department not found");

        if (_employeeRepo.AnyInDepartment(id))
            throw new Exception("Cannot delete department because it has active employees");

        _departmentRepo.Delete(id);
    }
    public async Task DeleteDepartmentAsync(int id)
    {
        if (id <= 0)
            throw new Exception("Invalid department Id");
        var dept = await _departmentRepo.GetByIdAsync(id);
        if (dept is null)
            throw new Exception("Department not found");
        if (await _employeeRepo.AnyInDepartmentAsync(id))
            throw new Exception("Cannot delete department because it has active employees");
        await _departmentRepo.DeleteAsync(id);
    }
    public DepartmentTotals GetDepartmentTotalsById(int departmentId)
    {
        if (departmentId <= 0)
            throw new ArgumentException("Invalid department id.");

        var totals = _departmentRepo.GetDepartmentTotalsById(departmentId);

        if (totals == null)
            throw new Exception("Department not found.");

        return totals;
    }
    public async Task<DepartmentTotals> GetDepartmentTotalsByIdAsync(int departmentId)
    {
        if (departmentId <= 0)
            throw new ArgumentException("Invalid department id.");
        var totals = await _departmentRepo.GetDepartmentTotalsByIdAsync(departmentId);
        if (totals == null)
            throw new Exception("Department not found.");
        return totals;
    }
    public DepartmentTotals GetDepartmentTotalsById_StoredProcedure(int departmentId)
    {
        if (departmentId <= 0)
            throw new ArgumentException("Invalid department id.");

        var totals = _departmentRepo.GetDepartmentTotalsById_SP(departmentId);

        if (totals == null)
            throw new Exception("Department not found.");

        return totals;
    }
    public async Task<DepartmentTotals> GetDepartmentTotalsById_StoredProcedureAsync(int departmentId)
    {
        if (departmentId <= 0)
            throw new ArgumentException("Invalid department id.");
        var totals = await _departmentRepo.GetDepartmentTotalsById_SPAsync(departmentId);
        if (totals == null)
            throw new Exception("Department not found.");
        return totals;
    }


}
