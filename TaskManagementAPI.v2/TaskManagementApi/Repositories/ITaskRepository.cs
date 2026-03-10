using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories;

public interface ITaskRepository
{
    Task<int> CreateAsync(TaskItem task);
    Task<List<TaskItem>> GetByUserIdAsync(int userId);
    Task<TaskItem?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(TaskItem task);
    Task<bool> DeleteAsync(int id);
}