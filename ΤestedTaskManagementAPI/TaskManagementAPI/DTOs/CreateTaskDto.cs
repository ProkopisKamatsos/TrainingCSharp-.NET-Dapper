using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.DTOs;

public class CreateTaskDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    [Required]
    [RegularExpression("^(Pending|InProgress|Completed|Cancelled)$",
        ErrorMessage = "Status must be one of: Pending, InProgress, Completed, Cancelled")]
    public string Status { get; set; } = "Pending";

    [Range(1, 5)]
    public int Priority { get; set; } = 3;

    [Required]
    public int UserId { get; set; }

    public DateTime? DueDate { get; set; }
}
