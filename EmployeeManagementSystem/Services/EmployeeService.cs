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
    public Employee UpdateEmployee(Employee employee)
    {
        if (employee.Id <= 0)
            throw new Exception("Invalid employee Id");

        var existing = _employeeRepository.GetById(employee.Id);
        if (existing == null)
            throw new Exception("Employee not found");

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
    public void DeleteEmployee(int id)
    {
        if (id <= 0)
            throw new Exception("Invalid employee Id");

        var employee = _employeeRepository.GetById(id);
        if (employee == null)
            throw new Exception("Employee not found");

        if (!employee.IsActive)
            throw new Exception("Employee is already inactive");

        _employeeRepository.SoftDelete(id);
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
    public Employee GetEmployeeWithDepartmentById(int id)
    {
        if (id <= 0)
            throw new Exception("Invalid employee Id");

        var employee = _employeeRepository.GetEmployeeWithDepartmentById(id);
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


}
