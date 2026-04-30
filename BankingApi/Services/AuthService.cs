using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BankingApi.DTOs.Auth;
using BankingApi.Models;
using BankingApi.Repositories.Interfaces;
using BankingApi.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace BankingApi.Services;

public class AuthService : IAuthService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        ICustomerRepository customerRepository,
        IConfiguration configuration,
        ILogger<AuthService> logger)
    {
        _customerRepository = customerRepository;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        // Ελέγχουμε αν υπάρχει ήδη customer με αυτό το email
        var existing = await _customerRepository.GetByEmailAsync(dto.Email);
        if (existing != null)
            throw new InvalidOperationException("Email already in use.");

        var customer = new Customer
        {
            FirstName    = dto.FirstName,
            LastName     = dto.LastName,
            Email        = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            PhoneNumber  = dto.PhoneNumber
        };

        var customerId = await _customerRepository.CreateAsync(customer);
        customer.CustomerId = customerId;

        _logger.LogInformation("New customer registered: {Email}", dto.Email);

        return new AuthResponseDto
        {
            Token    = GenerateToken(customer),
            FullName = $"{customer.FirstName} {customer.LastName}",
            Email    = customer.Email
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var customer = await _customerRepository.GetByEmailAsync(dto.Email);

        // Σκόπιμα το ίδιο μήνυμα για email και password
        // έτσι ο attacker δεν ξέρει αν το email υπάρχει ή όχι
        if (customer == null || !BCrypt.Net.BCrypt.Verify(dto.Password, customer.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials.");

        _logger.LogInformation("Customer logged in: {Email}", dto.Email);

        return new AuthResponseDto
        {
            Token    = GenerateToken(customer),
            FullName = $"{customer.FirstName} {customer.LastName}",
            Email    = customer.Email
        };
    }

    private string GenerateToken(Customer customer)
    {
        var key     = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds   = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, customer.CustomerId.ToString()),
            new Claim(ClaimTypes.Name,           customer.FirstName),
            new Claim(ClaimTypes.Email,          customer.Email)
        };

        var token = new JwtSecurityToken(
            issuer:             _configuration["Jwt:Issuer"],
            audience:           _configuration["Jwt:Audience"],
            claims:             claims,
            expires:            DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}