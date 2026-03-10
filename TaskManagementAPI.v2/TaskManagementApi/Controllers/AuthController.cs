using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.DTOs.Auth;
using TaskManagementApi.Services;

namespace TaskManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var success = await _authService.RegisterAsync(request);

        if (!success)
        {
            return BadRequest(new { message = "Email or username already exists." });
        }

        return StatusCode(StatusCodes.Status201Created, new
        {
            message = "User registered successfully."
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _authService.LoginAsync(request);

        if (response is null)
        {
            return Unauthorized(new { message = "Invalid email or password." });
        }

        return Ok(response);
    }
}