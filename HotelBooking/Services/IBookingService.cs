using HotelBooking.DTOs.Booking;

namespace HotelBooking.Services
{
    public interface IBookingService
    {
        Task<BookingResponseDto?> GetByIdAsync(int bookingId, int requestingUserId, string requestingUserRole);
        Task<IEnumerable<BookingResponseDto>> GetByUserIdAsync(int userId);
        Task<BookingResponseDto> CreateAsync(int userId, CreateBookingDto dto);
        Task CancelAsync(int bookingId, int requestingUserId, string requestingUserRole);
    }
}