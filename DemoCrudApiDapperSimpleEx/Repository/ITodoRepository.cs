using WebApplication1.Data;

namespace WebApplication1.Repository
{
    public interface ITodoRepository
    {
        Task<IEnumerable<TodoDTO>> GetAllAsync();
        Task<TodoDTO> CreateAsync(CreateTodoDTO dto);
        Task<TodoDTO?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, UpdateTodoDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
