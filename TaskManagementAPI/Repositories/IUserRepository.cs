using TaskManagementAPI.Models;

namespace TaskManagementAPI.Repositories;

public interface IUserRepository
{
    Task<int> CreateAsync(User user);
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task<bool> UpdateProfileAsync(int id, string? firstName, string? lastName);
    Task<bool> DeactivateAsync(int id);




}
