using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;

public class EmployeeService
{
    private readonly EmployeeRepository _employeeRepository;
    private readonly DepartmentRepository _departmentRepository;

    public EmployeeService(
        EmployeeRepository employeeRepository,
        DepartmentRepository departmentRepository)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
    }

    public Employee CreateEmployee(Employee employee)
    {

        if (string.IsNullOrWhiteSpace(employee.FirstName))
            throw new Exception("First name is required");

        if (string.IsNullOrWhiteSpace(employee.LastName))
            throw new Exception("Last name is required");

        if (string.IsNullOrWhiteSpace(employee.Email) || !employee.Email.Contains("@"))
            throw new Exception("Invalid email");

        if (employee.Salary < 0)
            throw new Exception("Salary cannot be negative");

        if (!_departmentRepository.Exists(employee.DepartmentId))
            throw new Exception("Department does not exist");

        return _employeeRepository.Insert(employee);
    }
    public async Task<Employee> CreateEmployeeAsync(Employee employee)
    {
        if (string.IsNullOrWhiteSpace(employee.FirstName))
            throw new Exception("First name is required");
        if (string.IsNullOrWhiteSpace(employee.LastName))
            throw new Exception("Last name is required");
        if (string.IsNullOrWhiteSpace(employee.Email) || !employee.Email.Contains("@"))
            throw new Exception("Invalid email");
        if (employee.Salary < 0)
            throw new Exception("Salary cannot be negative");
        if (!await _departmentRepository.ExistsAsync(employee.DepartmentId))
            throw new Exception("Department does not exist");
        return await _employeeRepository.InsertAsync(employee);
    }
    public Employee UpdateEmployee(Employee employee)
    {
        if (employee.Id <= 0)
            throw new Exception("Invalid employee Id");

        var existing = _employeeRepository.GetById(employee.Id);
        if (existing == null)
            throw new Exception("Employee not found");
        var activeEmployee = _employeeRepository.GetById(employee.Id);
        if (activeEmployee != null && !activeEmployee.IsActive)
            throw new Exception("Cannot update an inactive employee");

        if (string.IsNullOrWhiteSpace(employee.FirstName))
            throw new Exception("First name is required");

        if (string.IsNullOrWhiteSpace(employee.LastName))
            throw new Exception("Last name is required");

        if (string.IsNullOrWhiteSpace(employee.Email) || !employee.Email.Contains("@"))
            throw new Exception("Invalid email");

        if (employee.Salary < 0)
            throw new Exception("Salary cannot be negative");

        if (!_departmentRepository.Exists(employee.DepartmentId))
            throw new Exception("Department does not exist");

        return _employeeRepository.Update(employee);
    }
    public async Task<Employee> UpdateEmployeeAsync(Employee employee)
    {
        if (employee.Id <= 0)
            throw new Exception("Invalid employee Id");
        var existing = await _employeeRepository.GetByIdAsync(employee.Id);
        if (existing == null)
            throw new Exception("Employee not found");
        if (!existing.IsActive)
            throw new Exception("Cannot update an inactive employee");
        if (string.IsNullOrWhiteSpace(employee.FirstName))
            throw new Exception("First name is required");
        if (string.IsNullOrWhiteSpace(employee.LastName))
            throw new Exception("Last name is required");
        if (string.IsNullOrWhiteSpace(employee.Email) || !employee.Email.Contains("@"))
            throw new Exception("Invalid email");
        if (employee.Salary < 0)
            throw new Exception("Salary cannot be negative");
        if (!await _departmentRepository.ExistsAsync(employee.DepartmentId))
            throw new Exception("Department does not exist");
        return await _employeeRepository.UpdateAsync(employee);
    }
    public void DeleteEmployee(int id)
    {
        if (id <= 0)
            throw new Exception("Invalid employee Id");

        var employee = _employeeRepository.GetById(id);
        if (employee == null)
            throw new Exception("Employee not found");

        if (!employee.IsActive)
            throw new Exception("Employee is already inactive.");

        _employeeRepository.SoftDelete(id);
    }
    public async Task DeleteEmployeeAsync(int id)
    {
        if (id <= 0) throw new Exception("Invalid employee Id");

        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null) throw new Exception("Employee not found");
        if (!employee.IsActive) throw new Exception("Employee is already inactive");

        await _employeeRepository.SoftDeleteAsync(id);
    }

    public List<Employee> SearchEmployees(EmployeeSearchCriteria criteria)
    {
        if (criteria.SalaryFrom.HasValue && criteria.SalaryTo.HasValue)
        {
            if (criteria.SalaryFrom.Value > criteria.SalaryTo.Value)
                throw new Exception("SalaryFrom cannot be greater than SalaryTo");
        }

        return _employeeRepository.Search(criteria);
    }
    public async Task<List<Employee>> SearchEmployeesAsync(EmployeeSearchCriteria criteria)
    {
        if (criteria.SalaryFrom.HasValue && criteria.SalaryTo.HasValue)
        {
            if (criteria.SalaryFrom.Value > criteria.SalaryTo.Value)
                throw new Exception("SalaryFrom cannot be greater than SalaryTo");
        }
        return await _employeeRepository.SearchAsync(criteria);
    }
    public Employee GetEmployeeWithDepartmentById(int id)
    {
        if (id <= 0)
            throw new Exception("Invalid employee Id");

        var employee = _employeeRepository.GetEmployeeWithDepartmentById(id);
        if (employee == null)
            throw new Exception("Employee not found");

        return employee;
    }
    public async Task<Employee> GetEmployeeWithDepartmentByIdAsync(int id)
    {
        if (id <= 0)
            throw new Exception("Invalid employee Id");
        var employee = await _employeeRepository.GetEmployeeWithDepartmentByIdAsync(id);
        if (employee == null)
            throw new Exception("Employee not found");
        return employee;
    }
    public Employee GetEmployeeWithProjectsById(int id)
    {
        if (id <= 0) throw new Exception("Invalid employee Id");

        var emp = _employeeRepository.GetEmployeeWithProjectsById(id);
        if (emp is null) throw new Exception("Employee not found");

        return emp;
    }
    public async Task<Employee> GetEmployeeWithProjectsByIdAsync(int id)
    {
        if (id <= 0) throw new Exception("Invalid employee Id");
        var emp = await _employeeRepository.GetEmployeeWithProjectsByIdAsync(id);
        if (emp is null) throw new Exception("Employee not found");
        return emp;
    }
    public void UpdateEmployeeSalary(int employeeId, decimal newSalary)
    {
        if (employeeId <= 0)
            throw new ArgumentException("Invalid employee id.");

        if (newSalary < 0)
            throw new ArgumentException("Salary cannot be negative.");

        _employeeRepository.UpdateSalaryWithHistory(employeeId, newSalary);
    }
    public async Task UpdateEmployeeSalaryAsync(int employeeId, decimal newSalary)
    {
        if (employeeId <= 0)
            throw new ArgumentException("Invalid employee id.");
        if (newSalary < 0)
            throw new ArgumentException("Salary cannot be negative.");
        await _employeeRepository.UpdateSalaryWithHistoryAsync(employeeId, newSalary);
    }





}
