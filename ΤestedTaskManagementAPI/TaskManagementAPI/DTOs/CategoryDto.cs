namespace TaskManagementAPI.DTOs;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Color { get; set; }
    public string? Icon { get; set; }
}
