using HotelBooking.DTOs.Payment;

namespace HotelBooking.Services
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto> GetByIdAsync(int paymentId, int requestingUserId, string requestingUserRole);
        Task<PaymentResponseDto> GetByBookingIdAsync(int bookingId, int requestingUserId, string requestingUserRole);
        Task<PaymentResponseDto> CreateAsync(int requestingUserId, string requestingUserRole, CreatePaymentDto dto);
    }
}