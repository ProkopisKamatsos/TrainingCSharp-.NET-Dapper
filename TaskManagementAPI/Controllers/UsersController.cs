using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;
using TaskManagementAPI.Repositories;

namespace TaskManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _users;

    public UsersController(IUserRepository users)
    {
        _users = users;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (await _users.GetByEmailAsync(dto.Email) != null)
            return BadRequest("Email already exists");

        if (await _users.GetByUsernameAsync(dto.Username) != null)
            return BadRequest("Username already exists");

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            FirstName = dto.FirstName,
            LastName = dto.LastName
        };

        var newId = await _users.CreateAsync(user);

        return CreatedAtAction(nameof(GetUser), new { id = newId }, new { userId = newId });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _users.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        // Δεν επιστρέφουμε PasswordHash
        return Ok(new
        {
            user.Id,
            user.Username,
            user.Email,
            user.FirstName,
            user.LastName,
            user.CreatedAt,
            user.IsActive
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProfile(int id, [FromBody] UpdateUserWithCredentialsDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _users.GetByEmailAsync(dto.Email);
        if (user == null || !user.IsActive)
            return Unauthorized("Invalid credentials");

       
        if (user.Id != id)
            return Forbid(); 

        
        var ok = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!ok)
            return Unauthorized("Invalid credentials");

       
        var updated = await _users.UpdateProfileAsync(id, dto.FirstName, dto.LastName);
        if (!updated)
            return NotFound();

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _users.GetByEmailAsync(dto.Email);

        if (user == null || !user.IsActive)
            return Unauthorized("Invalid credentials");

        var passwordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

        if (!passwordValid)
            return Unauthorized("Invalid credentials");

        return Ok(new
        {
            user.Id,
            user.Username,
            user.Email,
            user.FirstName,
            user.LastName
        });
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Deactivate(int id, [FromBody] DeleteUserDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _users.GetByEmailAsync(dto.Email);
        if (user == null || !user.IsActive)
            return Unauthorized("Invalid credentials");

        if (user.Id != id)
            return Forbid();

        var ok = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!ok)
            return Unauthorized("Invalid credentials");

        var deactivated = await _users.DeactivateAsync(id);
        if (!deactivated)
            return NotFound();

        return NoContent(); 
    }



}
