using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.DTOs;

public class CreateCommentDto
{
    [Required]
    public int UserId { get; set; }

    [Required]
    [MinLength(1)]
    public string Content { get; set; } = null!;
}
