using TaskManagementApi.DTOs.Users;

namespace TaskManagementApi.Services;

public interface IUserService
{
    Task<UserProfileDto?> GetByIdAsync(int id);
    Task<UserProfileDto?> UpdateProfileAsync(int id, UpdateUserDto request);
    Task<bool> DeactivateAsync(int id);
}