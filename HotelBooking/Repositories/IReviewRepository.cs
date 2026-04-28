using HotelBooking.Models;

namespace HotelBooking.Repositories
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetByHotelIdAsync(int hotelId);
        Task<Review?> GetByBookingIdAsync(int bookingId);
        Task<int> CreateAsync(Review review);
    }
}