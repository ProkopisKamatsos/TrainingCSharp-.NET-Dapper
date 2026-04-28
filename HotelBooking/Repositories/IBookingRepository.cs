using HotelBooking.Models;

namespace HotelBooking.Repositories
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(int id);
        Task<IEnumerable<Booking>> GetByUserIdAsync(int userId);
        Task<bool> HasOverlapAsync(int roomId, DateOnly checkIn, DateOnly checkOut);
        Task<int> CreateAsync(Booking booking);
        Task CancelAsync(int id);
    }
}