using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;

namespace EmployeeManagement.Tests.TestHelpers
{
    public class FakeEmployeeRepository : IEmployeeRepository
    {
        private readonly List<Employee> _employees = new();
        private int _nextId = 1;


        public void Seed(params Employee[] employees)
        {
            foreach (var e in employees)
            {
                if (e.Id <= 0)
                    e.Id = _nextId++;

                _nextId = Math.Max(_nextId, e.Id + 1);
                _employees.Add(Clone(e));
            }
        }

        public Employee? GetById(int id)
            => _employees.SingleOrDefault(e => e.Id == id);

        public Task<Employee?> GetByIdAsync(int id)
            => Task.FromResult(GetById(id));

        public List<Employee> GetAll()
            => _employees.Select(Clone).ToList();

        public Task<List<Employee>> GetAllAsync()
            => Task.FromResult(GetAll());

        public Employee Insert(Employee employee)
        {
            var copy = Clone(employee);
            copy.Id = _nextId++;
            _employees.Add(copy);

            employee.Id = copy.Id;
            return employee;
        }

        public Task<Employee> InsertAsync(Employee employee)
            => Task.FromResult(Insert(employee));

        public Employee Update(Employee employee)
        {
            var idx = _employees.FindIndex(e => e.Id == employee.Id);
            if (idx < 0) throw new Exception("Employee not found.");

            _employees[idx] = Clone(employee);
            return employee;
        }

        public Task<Employee> UpdateAsync(Employee employee)
            => Task.FromResult(Update(employee));

        public void SoftDelete(int id)
        {
            var emp = _employees.SingleOrDefault(e => e.Id == id);
            if (emp == null) throw new Exception("Employee not found.");

            emp.IsActive = false;
        }

        public Task SoftDeleteAsync(int id)
        {
            SoftDelete(id);
            return Task.CompletedTask;
        }

        public bool AnyInDepartment(int departmentId)
            => _employees.Any(e => e.DepartmentId == departmentId && e.IsActive);

        public Task<bool> AnyInDepartmentAsync(int departmentId)
            => Task.FromResult(AnyInDepartment(departmentId));

        public List<Employee> Search(EmployeeSearchCriteria criteria)
        {
            IEnumerable<Employee> q = _employees.Where(e => e.IsActive);

            if (!string.IsNullOrWhiteSpace(criteria.Name))
            {
                var name = criteria.Name.Trim();
                q = q.Where(e =>
                    (e.FirstName?.Contains(name, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (e.LastName?.Contains(name, StringComparison.OrdinalIgnoreCase) ?? false));
            }

            if (criteria.DepartmentId.HasValue)
                q = q.Where(e => e.DepartmentId == criteria.DepartmentId.Value);

            if (criteria.SalaryFrom.HasValue && criteria.SalaryTo.HasValue)
                q = q.Where(e => e.Salary >= criteria.SalaryFrom.Value && e.Salary <= criteria.SalaryTo.Value);

            return q.Select(Clone).ToList();
        }

        public Task<List<Employee>> SearchAsync(EmployeeSearchCriteria criteria)
            => Task.FromResult(Search(criteria));


        public Employee? GetEmployeeWithDepartmentById(int employeeId) => throw new NotImplementedException();
        public Task<Employee?> GetEmployeeWithDepartmentByIdAsync(int employeeId) => throw new NotImplementedException();
        public Employee? GetEmployeeWithProjectsById(int employeeId) => throw new NotImplementedException();
        public Task<Employee?> GetEmployeeWithProjectsByIdAsync(int employeeId) => throw new NotImplementedException();
        public void UpdateSalaryWithHistory(int employeeId, decimal newSalary) => throw new NotImplementedException();
        public Task UpdateSalaryWithHistoryAsync(int employeeId, decimal newSalary) => throw new NotImplementedException();
        public Task BulkInsertAsync(List<Employee> employees) => throw new NotImplementedException();

        private static Employee Clone(Employee e) => new Employee
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            Phone = e.Phone,
            DepartmentId = e.DepartmentId,
            Salary = e.Salary,
            HireDate = e.HireDate,
            IsActive = e.IsActive,
            Department = e.Department,
            ProjectAssignments = e.ProjectAssignments
        };
    }
}