using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.DTOs;

public class CreateCategoryDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(20)]
    public string? Color { get; set; }

    [StringLength(50)]
    public string? Icon { get; set; }
}
