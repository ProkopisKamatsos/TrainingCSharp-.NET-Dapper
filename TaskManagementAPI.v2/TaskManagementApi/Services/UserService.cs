using TaskManagementApi.DTOs.Users;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserProfileDto?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null || !user.IsActive)
        {
            return null;
        }

        return new UserProfileDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        };
    }
    public async Task<UserProfileDto?> UpdateProfileAsync(int id, UpdateUserDto request)
    {
        var existingUser = await _userRepository.GetByIdAsync(id);

        if (existingUser is null || !existingUser.IsActive)
        {
            return null;
        }

        var userWithSameEmail = await _userRepository.GetByEmailAsync(request.Email);
        if (userWithSameEmail is not null && userWithSameEmail.Id != id)
        {
            return null;
        }

        var userWithSameUsername = await _userRepository.GetByUsernameAsync(request.Username);
        if (userWithSameUsername is not null && userWithSameUsername.Id != id)
        {
            return null;
        }

        existingUser.Username = request.Username.Trim();
        existingUser.Email = request.Email.Trim();
        existingUser.FirstName = request.FirstName?.Trim();
        existingUser.LastName = request.LastName?.Trim();
        existingUser.UpdatedAt = DateTime.UtcNow;

        var updated = await _userRepository.UpdateAsync(existingUser);

        if (!updated)
        {
            return null;
        }

        return new UserProfileDto
        {
            Id = existingUser.Id,
            Username = existingUser.Username,
            Email = existingUser.Email,
            FirstName = existingUser.FirstName,
            LastName = existingUser.LastName,
            Role = existingUser.Role,
            IsActive = existingUser.IsActive,
            CreatedAt = existingUser.CreatedAt
        };
    }
    public async Task<bool> DeactivateAsync(int id)
    {
        var existingUser = await _userRepository.GetByIdAsync(id);

        if (existingUser is null || !existingUser.IsActive)
        {
            return false;
        }

        return await _userRepository.DeactivateAsync(id);
    }
}