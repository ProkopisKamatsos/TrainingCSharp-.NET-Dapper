using Moq;
using Xunit;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using EmployeeManagement.Tests.TestHelpers;

namespace EmployeeManagement.Tests.Services
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeRepository> _empRepo;
        private readonly Mock<IDepartmentRepository> _deptRepo;
        private readonly EmployeeService _service;

        public EmployeeServiceTests()
        {
            _empRepo = new Mock<IEmployeeRepository>();
            _deptRepo = new Mock<IDepartmentRepository>();
            _service = new EmployeeService(_empRepo.Object, _deptRepo.Object);
        }

        [Fact]
        public void CreateEmployee_ValidEmployee_CallsInsertOnce()
        {
            // Arrange
            var employee = MockData.ValidEmployee();
            _deptRepo.Setup(d => d.Exists(employee.DepartmentId)).Returns(true);
            _empRepo.Setup(r => r.Insert(employee)).Returns(employee);

            // Act
            var result = _service.CreateEmployee(employee);

            // Assert
            _empRepo.Verify(r => r.Insert(employee), Times.Once);
            Assert.Equal(employee.Email, result.Email);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CreateEmployee_InvalidFirstName_ThrowsException(string firstName)
        {
            // Arrange
            var employee = MockData.ValidEmployee();
            employee.FirstName = firstName;

            // Act + Assert
            var ex = Assert.Throws<Exception>(() => _service.CreateEmployee(employee));
            Assert.Equal("First name is required", ex.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("not-an-email")]
        public void CreateEmployee_InvalidEmail_ThrowsException(string email)
        {
            // Arrange
            var employee = MockData.ValidEmployee();
            employee.Email = email;

            // Act + Assert
            var ex = Assert.Throws<Exception>(() => _service.CreateEmployee(employee));
            Assert.Equal("Invalid email", ex.Message);
        }

        [Fact]
        public void CreateEmployee_DepartmentDoesNotExist_ThrowsException()
        {
            // Arrange
            var employee = MockData.ValidEmployee(departmentId: 99);
            _deptRepo.Setup(d => d.Exists(employee.DepartmentId)).Returns(false);

            // Act + Assert
            var ex = Assert.Throws<Exception>(() => _service.CreateEmployee(employee));
            Assert.Equal("Department does not exist", ex.Message);

            
            _empRepo.Verify(r => r.Insert(It.IsAny<Employee>()), Times.Never);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_InactiveEmployee_ThrowsException()
        {
            // Arrange
            var employeeToUpdate = MockData.ValidEmployee(id: 1);
            var existing = MockData.ValidEmployee(id: 1);
            existing.IsActive = false;

            _empRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

            // Act + Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.UpdateEmployeeAsync(employeeToUpdate));
            Assert.Equal("Cannot update an inactive employee", ex.Message);
        }

        [Fact]
        public async Task DeleteEmployeeAsync_AlreadyInactive_ThrowsException()
        {
            // Arrange
            var existing = MockData.ValidEmployee(id: 2);
            existing.IsActive = false;

            _empRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(existing);

            // Act + Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.DeleteEmployeeAsync(2));
            Assert.Equal("Employee is already inactive", ex.Message);
        }

        [Fact]
        public void SearchEmployees_SalaryFromGreaterThanSalaryTo_ThrowsException()
        {
            // Arrange
            var criteria = new EmployeeSearchCriteria
            {
                SalaryFrom = 2000,
                SalaryTo = 1000
            };

            // Act + Assert
            var ex = Assert.Throws<Exception>(() => _service.SearchEmployees(criteria));
            Assert.Equal("SalaryFrom cannot be greater than SalaryTo", ex.Message);
        }

        [Theory]
        [InlineData(0, 100)]
        [InlineData(-1, 100)]
        public void UpdateEmployeeSalary_InvalidEmployeeId_ThrowsArgumentException(int employeeId, decimal newSalary)
        {
            // Act + Assert
            var ex = Assert.Throws<ArgumentException>(() => _service.UpdateEmployeeSalary(employeeId, newSalary));
            Assert.Equal("Invalid employee id.", ex.Message);
        }

        [Fact]
        public void UpdateEmployeeSalary_NegativeSalary_ThrowsArgumentException()
        {
            // Act + Assert
            var ex = Assert.Throws<ArgumentException>(() => _service.UpdateEmployeeSalary(1, -10));
            Assert.Equal("Salary cannot be negative.", ex.Message);
        }
    }
}