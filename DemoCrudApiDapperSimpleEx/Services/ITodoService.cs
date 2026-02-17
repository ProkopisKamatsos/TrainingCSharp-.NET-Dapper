using WebApplication1.Data;

namespace WebApplication1.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoDTO>> GetAllAsync();
        Task<TodoDTO> CreateAsync(CreateTodoDTO dto);
        Task<TodoDTO?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, UpdateTodoDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
