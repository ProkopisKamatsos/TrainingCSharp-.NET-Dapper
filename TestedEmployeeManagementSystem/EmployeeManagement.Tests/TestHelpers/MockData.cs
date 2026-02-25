using EmployeeManagementSystem.Models;

namespace EmployeeManagement.Tests.TestHelpers
{
    public static class MockData
    {
        public static Employee ValidEmployee(int id = 0, int departmentId = 1)
            => new Employee
            {
                Id = id,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@doe.com",
                Salary = 1000,
                DepartmentId = departmentId,
                IsActive = true
            };
    }
}