using TaskManagementApi.DTOs.Tasks;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<TaskResponseDto?> CreateAsync(int userId, CreateTaskDto request)
    {
        var now = DateTime.UtcNow;

        var taskItem = new TaskItem
        {
            Title = request.Title.Trim(),
            Description = request.Description?.Trim(),
            Status = request.Status.Trim(),
            Priority = request.Priority,
            UserId = userId,
            DueDate = request.DueDate,
            CreatedAt = now,
            UpdatedAt = now,
            CompletedAt = request.Status == "Completed" ? now : null
        };

        var newTaskId = await _taskRepository.CreateAsync(taskItem);

        if (newTaskId <= 0)
        {
            return null;
        }

        taskItem.Id = newTaskId;

        return MapToResponse(taskItem);
    }

    public async Task<List<TaskResponseDto>> GetMyTasksAsync(int userId)
    {
        var tasks = await _taskRepository.GetByUserIdAsync(userId);

        return tasks
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<TaskResponseDto?> GetByIdAsync(int taskId, int userId)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);

        if (task is null)
        {
            return null;
        }

        if (task.UserId != userId)
        {
            return null;
        }

        return MapToResponse(task);
    }

    private static TaskResponseDto MapToResponse(TaskItem task)
    {
        return new TaskResponseDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            UserId = task.UserId,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
            CompletedAt = task.CompletedAt
        };
    }
    public async Task<TaskResponseDto?> UpdateAsync(int taskId, int userId, UpdateTaskDto request)
    {
        var existingTask = await _taskRepository.GetByIdAsync(taskId);

        if (existingTask is null)
        {
            return null;
        }

        if (existingTask.UserId != userId)
        {
            return null;
        }

        existingTask.Title = request.Title.Trim();
        existingTask.Description = request.Description?.Trim();
        existingTask.Status = request.Status.Trim();
        existingTask.Priority = request.Priority;
        existingTask.DueDate = request.DueDate;
        existingTask.UpdatedAt = DateTime.UtcNow;
        existingTask.CompletedAt = request.Status == "Completed"
            ? existingTask.CompletedAt ?? DateTime.UtcNow
            : null;

        var updated = await _taskRepository.UpdateAsync(existingTask);

        if (!updated)
        {
            return null;
        }

        return MapToResponse(existingTask);
    }
    public async Task<bool> DeleteAsync(int taskId, int userId)
    {
        var existingTask = await _taskRepository.GetByIdAsync(taskId);

        if (existingTask is null)
        {
            return false;
        }

        if (existingTask.UserId != userId)
        {
            return false;
        }

        return await _taskRepository.DeleteAsync(taskId);
    }
}