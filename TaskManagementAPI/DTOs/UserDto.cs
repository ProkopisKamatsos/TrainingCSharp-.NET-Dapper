using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.DTOs;

public class UserDto
{
    public int Id { get; set; } 

    [Required]
    [StringLength(50)]
    public string Username { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = null!;

    [StringLength(50)]
    public string? FirstName { get; set; }

    [StringLength(50)]
    public string? LastName { get; set; }
}
