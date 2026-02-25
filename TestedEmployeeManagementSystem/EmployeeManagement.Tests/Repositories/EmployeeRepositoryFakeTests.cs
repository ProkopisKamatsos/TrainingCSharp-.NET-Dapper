using Xunit;
using EmployeeManagement.Tests.TestHelpers;
using EmployeeManagementSystem.Models;

namespace EmployeeManagement.Tests.Repositories
{
    public class EmployeeRepositoryFakeTests
    {
        [Fact]
        public async Task GetByIdAsync_ExistingEmployee_ReturnsEmployee()
        {
            // Arrange
            var repo = new FakeEmployeeRepository();
            repo.Seed(new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@test.com",
                DepartmentId = 1,
                Salary = 1000,
                IsActive = true
            });

            // Act
            var emp = await repo.GetByIdAsync(1);

            // Assert
            Assert.NotNull(emp);
            Assert.Equal(1, emp!.Id);
            Assert.Equal("john@test.com", emp.Email);
        }

        [Fact]
        public async Task InsertAsync_ValidEmployee_AssignsIdAndStoresEmployee()
        {
            // Arrange
            var repo = new FakeEmployeeRepository();
            var employee = new Employee
            {
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@test.com",
                DepartmentId = 2,
                Salary = 1500,
                IsActive = true
            };

            // Act
            var inserted = await repo.InsertAsync(employee);

            // Assert
            Assert.True(inserted.Id > 0);

            var fromRepo = await repo.GetByIdAsync(inserted.Id);
            Assert.NotNull(fromRepo);
            Assert.Equal("jane@test.com", fromRepo!.Email);
        }

        [Fact]
        public async Task SoftDeleteAsync_SetsIsActiveFalse()
        {
            // Arrange
            var repo = new FakeEmployeeRepository();
            repo.Seed(new Employee
            {
                Id = 5,
                FirstName = "Mark",
                LastName = "Smith",
                Email = "mark@test.com",
                DepartmentId = 1,
                Salary = 900,
                IsActive = true
            });

            // Act
            await repo.SoftDeleteAsync(5);

            // Assert
            var emp = await repo.GetByIdAsync(5);
            Assert.NotNull(emp);
            Assert.False(emp!.IsActive);
        }

        [Fact]
        public async Task SearchAsync_ByName_ReturnsOnlyMatches()
        {
            // Arrange
            var repo = new FakeEmployeeRepository();
            repo.Seed(
                new Employee { FirstName = "Alex", LastName = "Brown", Email = "a@test.com", DepartmentId = 1, Salary = 1000, IsActive = true },
                new Employee { FirstName = "Alexandra", LastName = "White", Email = "b@test.com", DepartmentId = 1, Salary = 2000, IsActive = true },
                new Employee { FirstName = "Bob", LastName = "Green", Email = "c@test.com", DepartmentId = 1, Salary = 1500, IsActive = true }
            );

            var criteria = new EmployeeSearchCriteria { Name = "Alex" };

            // Act
            var results = await repo.SearchAsync(criteria);

            // Assert
            Assert.Equal(2, results.Count);
        }
    }
}