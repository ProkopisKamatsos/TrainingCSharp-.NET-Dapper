using TaskManagementAPI.DTOs;

namespace TaskManagementAPI.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<CategoryDto>> GetAllAsync();
    Task<int> CreateAsync(CreateCategoryDto dto);
    Task<bool> UpdateAsync(int id, UpdateCategoryDto dto);
    Task<bool> DeleteAsync(int id);
}
