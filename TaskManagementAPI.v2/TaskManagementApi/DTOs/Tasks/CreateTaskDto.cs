using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs.Tasks;

public class CreateTaskDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    [RegularExpression("Pending|InProgress|Completed|Cancelled")]
    public string Status { get; set; } = "Pending";

    [Range(1, 5)]
    public int Priority { get; set; }

    public DateTime? DueDate { get; set; }
}