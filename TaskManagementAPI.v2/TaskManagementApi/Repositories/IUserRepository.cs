using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByIdAsync(int id);
    Task<int> CreateAsync(User user);
    Task<bool> UpdateAsync(User user);
    Task<bool> DeactivateAsync(int id);
}  