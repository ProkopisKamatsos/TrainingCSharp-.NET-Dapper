using BCrypt.Net;
using TaskManagementApi.DTOs.Auth;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;
using TaskManagementApi.Security;

namespace TaskManagementApi.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public AuthService(IUserRepository userRepository, JwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<bool> RegisterAsync(RegisterRequestDto request)
    {
        var existingEmail = await _userRepository.GetByEmailAsync(request.Email);
        if (existingEmail is not null)
        {
            return false;
        }

        var existingUsername = await _userRepository.GetByUsernameAsync(request.Username);
        if (existingUsername is not null)
        {
            return false;
        }

        var user = new User
        {
            Username = request.Username.Trim(),
            Email = request.Email.Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FirstName = request.FirstName?.Trim(),
            LastName = request.LastName?.Trim(),
            Role = "User",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var userId = await _userRepository.CreateAsync(user);

        return userId > 0;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user is null)
        {
            return null;
        }

        if (!user.IsActive)
        {
            return null;
        }

        var passwordIsValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!passwordIsValid)
        {
            return null;
        }

        var tokenResult = _jwtTokenGenerator.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = tokenResult.Token,
            ExpiresAt = tokenResult.ExpiresAt,
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        };
    }
}