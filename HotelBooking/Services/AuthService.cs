using BCrypt.Net;
using HotelBooking.DTOs.Auth;
using HotelBooking.Helpers;
using HotelBooking.Models;
using HotelBooking.Repositories;

namespace HotelBooking.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtHelper _jwtHelper;

        public AuthService(IUserRepository userRepository, JwtHelper jwtHelper)
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            // Ελέγχουμε αν υπάρχει ήδη user με αυτό το email
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new ArgumentException("An account with this email already exists.");

            // Δημιουργούμε τον νέο user
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "Guest",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            user.Id = await _userRepository.CreateAsync(user);

            return new AuthResponseDto
            {
                Token = _jwtHelper.GenerateToken(user),
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            // Ελέγχουμε αν υπάρχει ο user
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
                throw new ArgumentException("Invalid credentials.");

            // Ελέγχουμε το password
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new ArgumentException("Invalid credentials.");

            return new AuthResponseDto
            {
                Token = _jwtHelper.GenerateToken(user),
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}