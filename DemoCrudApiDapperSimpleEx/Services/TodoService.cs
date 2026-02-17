using WebApplication1.Data;
using WebApplication1.Repository;

namespace WebApplication1.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repo;

        public TodoService(ITodoRepository repo)
        {
            _repo = repo;
        }

        public async Task<TodoDTO> CreateAsync(CreateTodoDTO dto)
        {
            
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name is required.");

            if (string.IsNullOrWhiteSpace(dto.Secret))
                throw new ArgumentException("Secret is required.");

           
            return await _repo.CreateAsync(dto);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0) return false;
            return await _repo.DeleteAsync(id);
        }

        public async Task<IEnumerable<TodoDTO>> GetAllAsync()
            =>await _repo.GetAllAsync();

        public async Task<TodoDTO?> GetByIdAsync(int id)
        {
            if (id == 0)
                throw new ArgumentException("Insert exisitng Id");
            if (id < 0)
                throw new ArgumentException("Insert valid Id");
            return await _repo.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(int id, UpdateTodoDTO dto)
        {
            if (id <= 0) return false;
            if (string.IsNullOrWhiteSpace(dto.Name)) return false;
            return await _repo.UpdateAsync(id, dto);
        }
    }
}
