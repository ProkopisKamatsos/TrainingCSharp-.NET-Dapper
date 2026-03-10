using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.DTOs.Users;
using TaskManagementApi.Services;

namespace TaskManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userIdClaim))
        {
            return Unauthorized(new { message = "Invalid token." });
        }

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Invalid user id in token." });
        }

        var user = await _userService.GetByIdAsync(userId);

        if (user is null)
        {
            return NotFound(new { message = "User not found." });
        }

        return Ok(user);
    }
    [Authorize]
    [HttpPut("me")]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateUserDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userIdClaim))
        {
            return Unauthorized(new { message = "Invalid token." });
        }

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Invalid user id in token." });
        }

        var updatedUser = await _userService.UpdateProfileAsync(userId, request);

        if (updatedUser is null)
        {
            return BadRequest(new { message = "Could not update profile. Email or username may already exist." });
        }

        return Ok(updatedUser);
    }
    [Authorize]
    [HttpDelete("me")]
    public async Task<IActionResult> DeactivateMyAccount()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userIdClaim))
        {
            return Unauthorized(new { message = "Invalid token." });
        }

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Invalid user id in token." });
        }

        var success = await _userService.DeactivateAsync(userId);

        if (!success)
        {
            return BadRequest(new { message = "Could not deactivate account." });
        }

        return Ok(new { message = "Account deactivated successfully." });
    }
}