using TaskManagementAPI.DTOs;

namespace TaskManagementAPI.Services;

public interface ITaskService
{
    Task<(bool Success, string? Error, int StatusCode, int? TaskId)> CreateAsync(CreateTaskDto dto);
    Task<TaskDto?> GetByIdAsync(int id);
    Task<(bool Success, string? Error, int StatusCode)> UpdateAsync(int id, UpdateTaskDto dto);
    Task<(bool Success, string? Error, int StatusCode)> DeleteAsync(int id);
    Task<IEnumerable<TaskDto>> GetByUserIdAsync(int userId);
    Task<(bool Success, string? Error, IEnumerable<TaskDto>? Tasks, int StatusCode)> GetByStatusAsync(string status);
    Task<(bool Success, string? Error, IEnumerable<TaskDto>? Tasks, int StatusCode)> SearchAsync(string keyword);




}
