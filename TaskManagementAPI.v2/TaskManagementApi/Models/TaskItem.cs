namespace TaskManagementApi.Models;

public class TaskItem
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string Status { get; set; } = "Pending";

    public int Priority { get; set; }

    public int UserId { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }
}