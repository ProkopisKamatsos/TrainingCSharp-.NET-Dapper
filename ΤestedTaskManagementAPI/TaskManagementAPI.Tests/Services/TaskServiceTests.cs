using Moq;
using Xunit;
using System.Threading.Tasks;

using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;
using TaskManagementAPI.Repositories;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Tests.Services;

public class TaskServiceTests
{
    [Fact]
    public async System.Threading.Tasks.Task CreateAsync_UserNotFound_Returns404_AndDoesNotCreateTask()
    {
        // Arrange
        var taskRepo = new Mock<ITaskRepository>();
        var userRepo = new Mock<IUserRepository>();

        var service = new TaskService(taskRepo.Object, userRepo.Object);

        var dto = new CreateTaskDto
        {
            Title = "Test task",
            Description = "desc",
            Status = "Pending",
            Priority = 3,
            UserId = 123,
            DueDate = null
        };

        userRepo.Setup(r => r.GetByIdAsync(123))
                .ReturnsAsync((User?)null);

        // Act
        var result = await service.CreateAsync(dto);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(404, result.StatusCode);

        taskRepo.Verify(r => r.CreateAsync(It.IsAny<CreateTaskDto>(), It.IsAny<DateTime?>()), Times.Never);
    }
}