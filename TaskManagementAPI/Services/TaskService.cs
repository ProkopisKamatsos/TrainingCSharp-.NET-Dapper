using TaskManagementAPI.DTOs;
using TaskManagementAPI.Repositories;

namespace TaskManagementAPI.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _tasks;
    private readonly IUserRepository _users;

    public TaskService(ITaskRepository tasks, IUserRepository users)
    {
        _tasks = tasks;
        _users = users;
    }

    public async Task<(bool Success, string? Error, int StatusCode, int? TaskId)> CreateAsync(CreateTaskDto dto)
    {
       
        dto.Title = dto.Title.Trim();
        if (string.IsNullOrWhiteSpace(dto.Title))
            return (false, "Title cannot be empty", 400, null);

       
        if (dto.DueDate.HasValue && dto.DueDate.Value < DateTime.UtcNow)
            return (false, "DueDate cannot be in the past", 400, null);

        
        var user = await _users.GetByIdAsync(dto.UserId);
        if (user == null)
            return (false, "User not found", 404, null);

        
        DateTime? completedAt = null;
        if (dto.Status == "Completed")
            completedAt = DateTime.UtcNow;

        var newId = await _tasks.CreateAsync(dto, completedAt);
        return (true, null, 201, newId);
    }


    public Task<TaskDto?> GetByIdAsync(int id)
        => _tasks.GetByIdAsync(id);
    public async Task<(bool Success, string? Error, int StatusCode)> UpdateAsync(int id, UpdateTaskDto dto)
    {
        
        dto.Title = dto.Title.Trim();
        if (string.IsNullOrWhiteSpace(dto.Title))
            return (false, "Title cannot be empty", 400);

        if (dto.DueDate.HasValue && dto.DueDate.Value < DateTime.UtcNow)
            return (false, "DueDate cannot be in the past", 400);

        var existing = await _tasks.GetByIdAsync(id);
        if (existing == null)
            return (false, "Task not found", 404);

        DateTime? completedAt = dto.Status == "Completed" ? DateTime.UtcNow : null;

        var ok = await _tasks.UpdateAsync(id, dto, completedAt);
        if (!ok)
            return (false, "Task not found", 404);

        return (true, null, 200);
    }
    public async Task<(bool Success, string? Error, int StatusCode)> DeleteAsync(int id)
    {
        var ok = await _tasks.DeleteAsync(id);

        if (!ok)
            return (false, "Task not found", 404);

        return (true, null, 204);
    }
    public Task<IEnumerable<TaskDto>> GetByUserIdAsync(int userId)
    => _tasks.GetByUserIdAsync(userId);
    public async Task<(bool Success, string? Error, IEnumerable<TaskDto>? Tasks, int StatusCode)>
       GetByStatusAsync(string status)
    {
        
        status = status.Trim();

        var allowed = new[] { "Pending", "InProgress", "Completed", "Cancelled" };
        if (!allowed.Contains(status))
            return (false, "Invalid status", null, 400);

        var tasks = await _tasks.GetByStatusAsync(status);
        return (true, null, tasks, 200);
    }
    public async Task<(bool Success, string? Error, IEnumerable<TaskDto>? Tasks, int StatusCode)> SearchAsync(string keyword)
    {
        keyword = (keyword ?? "").Trim();
        if (keyword.Length == 0)
            return (false, "keyword is required", null, 400);

        var tasks = await _tasks.SearchAsync(keyword);
        return (true, null, tasks, 200);
    }



}
