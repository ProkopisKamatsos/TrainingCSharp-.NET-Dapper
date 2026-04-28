using HotelBooking.DTOs.Payment;
using HotelBooking.Models;
using HotelBooking.Repositories;

namespace HotelBooking.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBookingRepository _bookingRepository;

        public PaymentService(IPaymentRepository paymentRepository, IBookingRepository bookingRepository)
        {
            _paymentRepository = paymentRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<PaymentResponseDto> GetByIdAsync(int paymentId, int requestingUserId, string requestingUserRole)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null)
                throw new KeyNotFoundException("Payment not found.");

            // Resource-level authorization
            if (requestingUserRole == "Guest")
            {
                var booking = await _bookingRepository.GetByIdAsync(payment.BookingId);
                if (booking!.UserId != requestingUserId)
                    throw new UnauthorizedAccessException("Δεν έχετε δικαίωμα να δείτε αυτό το payment.");
            }

            return MapToDto(payment);
        }

        public async Task<PaymentResponseDto> GetByBookingIdAsync(int bookingId, int requestingUserId, string requestingUserRole)
        {
            // Ελέγχουμε αν υπάρχει το booking
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
                throw new KeyNotFoundException("Booking not found.");

            // Resource-level authorization
            if (requestingUserRole == "Guest" && booking.UserId != requestingUserId)
                throw new UnauthorizedAccessException("Δεν έχετε δικαίωμα να δείτε αυτό το payment.");

            var payment = await _paymentRepository.GetByBookingIdAsync(bookingId);
            if (payment == null)
                throw new KeyNotFoundException("Payment not found.");

            return MapToDto(payment);
        }

        public async Task<PaymentResponseDto> CreateAsync(int requestingUserId, string requestingUserRole, CreatePaymentDto dto)
        {
            // Ελέγχουμε αν υπάρχει το booking
            var booking = await _bookingRepository.GetByIdAsync(dto.BookingId);
            if (booking == null)
                throw new KeyNotFoundException("Booking not found.");

            // Resource-level authorization
            if (requestingUserRole == "Guest" && booking.UserId != requestingUserId)
                throw new UnauthorizedAccessException("Δεν έχετε δικαίωμα να πληρώσετε αυτό το booking.");

            // Business rules
            if (booking.Status == "Cancelled")
                throw new ArgumentException("Cannot pay for a cancelled booking.");

            if (booking.Status == "Completed")
                throw new ArgumentException("This booking has already been completed.");

            // Ελέγχουμε αν υπάρχει ήδη payment
            var existingPayment = await _paymentRepository.GetByBookingIdAsync(dto.BookingId);
            if (existingPayment != null)
                throw new ArgumentException("This booking has already been paid.");

            var payment = new Payment
            {
                BookingId = dto.BookingId,
                Amount = booking.TotalPrice,
                Method = dto.Method,
                Status = "Completed",
                PaidAt = DateTime.UtcNow
            };

            payment.Id = await _paymentRepository.CreateAsync(payment);
            return MapToDto(payment);
        }

        private static PaymentResponseDto MapToDto(Payment payment)
        {
            return new PaymentResponseDto
            {
                Id = payment.Id,
                BookingId = payment.BookingId,
                Amount = payment.Amount,
                Method = payment.Method,
                Status = payment.Status,
                PaidAt = payment.PaidAt
            };
        }
    }
}