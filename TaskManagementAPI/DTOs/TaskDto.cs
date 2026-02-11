
namespace TaskManagementAPI.DTOs;


public class TaskDto
{
    public int TaskId { get; set; }          
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Status { get; set; }  = null!;  
    public int Priority { get; set; }
    public int UserId { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
