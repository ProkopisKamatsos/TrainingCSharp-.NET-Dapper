using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Repositories;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ICommentRepository _comments;
    private readonly IUserRepository _users;
    private readonly ITaskService _service;

    public TasksController(ITaskService service, ICommentRepository comments, IUserRepository users)
    {
        _service = service;
        _comments = comments;
        _users = users;
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (success, error, statusCode, taskId) = await _service.CreateAsync(dto);

        if (!success)
            return statusCode switch
            {
                404 => NotFound(error),
                _ => BadRequest(error)
            };

        return CreatedAtAction(nameof(GetById), new { id = taskId }, new { taskId });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await _service.GetByIdAsync(id);
        if (task == null)
            return NotFound();

        return Ok(task);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (success, error, status) = await _service.UpdateAsync(id, dto);

        if (success)
            return Ok();

        return status switch
        {
            404 => NotFound(error),
            400 => BadRequest(error),
            _ => BadRequest(error)
        };
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, error, status) = await _service.DeleteAsync(id);

        if (success)
            return NoContent();

        return status switch
        {
            404 => NotFound(error),
            _ => BadRequest(error)
        };
    }
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        var tasks = await _service.GetByUserIdAsync(userId);
        return Ok(tasks);
    }
    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(string status)
    {
        var (success, error, tasks, code) = await _service.GetByStatusAsync(status);

        if (!success)
            return BadRequest(error);

        return Ok(tasks);
    }
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string keyword)
    {
        var (success, error, tasks, status) = await _service.SearchAsync(keyword);

        if (!success)
            return BadRequest(error);

        return Ok(tasks);
    }
    [Tags("Comments")]

    [HttpGet("{taskId}/comments")]
    public async Task<IActionResult> GetComments(int taskId)
    {
        var task = await _service.GetByIdAsync(taskId);
        if (task == null) return NotFound("Task not found");

        var comments = await _comments.GetByTaskIdAsync(taskId);
        return Ok(comments);
    }

    [Tags("Comments")]

    [HttpPost("{taskId}/comments")]
    public async Task<IActionResult> AddComment(int taskId, [FromBody] CreateCommentDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        
        var task = await _service.GetByIdAsync(taskId);
        if (task == null) return NotFound("Task not found");

        
        var user = await _users.GetByIdAsync(dto.UserId);
        if (user == null) return NotFound("User not found");

        dto.Content = dto.Content.Trim();
        if (dto.Content.Length == 0) return BadRequest("Content cannot be empty");

        var id = await _comments.CreateAsync(taskId, dto);
        return StatusCode(201, new { id });
    }



}
