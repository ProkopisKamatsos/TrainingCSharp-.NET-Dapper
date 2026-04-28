using HotelBooking.Models;

namespace HotelBooking.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByIdAsync(int id);
        Task<Payment?> GetByBookingIdAsync(int bookingId);
        Task<int> CreateAsync(Payment payment);
        Task RefundAsync(int bookingId);
    }
}
