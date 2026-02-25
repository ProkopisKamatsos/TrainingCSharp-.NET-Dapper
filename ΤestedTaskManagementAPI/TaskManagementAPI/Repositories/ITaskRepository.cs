using TaskManagementAPI.DTOs;

namespace TaskManagementAPI.Repositories;

public interface ITaskRepository
{
    Task<int> CreateAsync(CreateTaskDto dto, DateTime? completedAt);
    Task<TaskDto?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, UpdateTaskDto dto, DateTime? completedAt);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<TaskDto>> GetByUserIdAsync(int userId);
    Task<IEnumerable<TaskDto>> GetByStatusAsync(string status);
    Task<IEnumerable<TaskDto>> SearchAsync(string keyword);




}
