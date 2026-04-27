using HotelBooking.Models;

namespace HotelBooking.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<int> CreateAsync(User user);
    }
}