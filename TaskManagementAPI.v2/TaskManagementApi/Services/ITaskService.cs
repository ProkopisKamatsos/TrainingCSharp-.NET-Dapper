using TaskManagementApi.DTOs.Tasks;

namespace TaskManagementApi.Services;

public interface ITaskService
{
    Task<TaskResponseDto?> CreateAsync(int userId, CreateTaskDto request);
    Task<List<TaskResponseDto>> GetMyTasksAsync(int userId);
    Task<TaskResponseDto?> GetByIdAsync(int taskId, int userId);
    Task<TaskResponseDto?> UpdateAsync(int taskId, int userId, UpdateTaskDto request);
    Task<bool> DeleteAsync(int taskId, int userId);
}