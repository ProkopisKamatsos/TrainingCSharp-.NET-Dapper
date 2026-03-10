using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.DTOs.Tasks;
using TaskManagementApi.Services;

namespace TaskManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetUserIdFromToken();
        if (userId is null)
        {
            return Unauthorized(new { message = "Invalid token." });
        }

        var createdTask = await _taskService.CreateAsync(userId.Value, request);

        if (createdTask is null)
        {
            return BadRequest(new { message = "Could not create task." });
        }

        return StatusCode(StatusCodes.Status201Created, createdTask);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyTasks()
    {
        var userId = GetUserIdFromToken();
        if (userId is null)
        {
            return Unauthorized(new { message = "Invalid token." });
        }

        var tasks = await _taskService.GetMyTasksAsync(userId.Value);

        return Ok(tasks);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = GetUserIdFromToken();
        if (userId is null)
        {
            return Unauthorized(new { message = "Invalid token." });
        }

        var task = await _taskService.GetByIdAsync(id, userId.Value);

        if (task is null)
        {
            return NotFound(new { message = "Task not found." });
        }

        return Ok(task);
    }

    private int? GetUserIdFromToken()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userIdClaim))
        {
            return null;
        }

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return null;
        }

        return userId;
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetUserIdFromToken();
        if (userId is null)
        {
            return Unauthorized(new { message = "Invalid token." });
        }

        var updatedTask = await _taskService.UpdateAsync(id, userId.Value, request);

        if (updatedTask is null)
        {
            return NotFound(new { message = "Task not found or access denied." });
        }

        return Ok(updatedTask);
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserIdFromToken();
        if (userId is null)
        {
            return Unauthorized(new { message = "Invalid token." });
        }

        var deleted = await _taskService.DeleteAsync(id, userId.Value);

        if (!deleted)
        {
            return NotFound(new { message = "Task not found or access denied." });
        }

        return Ok(new { message = "Task deleted successfully." });
    }




}