using TaskManagementApi.DTOs.Auth;

namespace TaskManagementApi.Services;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterRequestDto request);
    Task<AuthResponseDto?> LoginAsync(LoginRequestDto request);
}