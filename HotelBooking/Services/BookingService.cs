using HotelBooking.DTOs.Booking;
using HotelBooking.Models;
using HotelBooking.Repositories;

namespace HotelBooking.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomTypeRepository _roomTypeRepository;

        public BookingService(
            IBookingRepository bookingRepository,
            IRoomRepository roomRepository,
            IRoomTypeRepository roomTypeRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _roomTypeRepository = roomTypeRepository;
        }

        public async Task<BookingResponseDto> GetByIdAsync(int bookingId, int requestingUserId, string requestingUserRole)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
                throw new KeyNotFoundException("Booking not found.");

            // Resource-level authorization
            if (requestingUserRole == "Guest" && booking.UserId != requestingUserId)
                throw new UnauthorizedAccessException("Δεν έχετε δικαίωμα να δείτε αυτό το booking.");

            return MapToDto(booking);
        }

        public async Task<IEnumerable<BookingResponseDto>> GetByUserIdAsync(int userId)
        {
            var bookings = await _bookingRepository.GetByUserIdAsync(userId);
            return bookings.Select(MapToDto);
        }

        public async Task<BookingResponseDto> CreateAsync(int userId, CreateBookingDto dto)
        {
            // Ελέγχουμε αν υπάρχει το room
            var room = await _roomRepository.GetByIdAsync(dto.RoomId);
            if (room == null)
                throw new KeyNotFoundException("Room not found.");

            // Ελέγχουμε τις ημερομηνίες
            if (dto.CheckIn >= dto.CheckOut)
                throw new ArgumentException("CheckIn date must be before CheckOut date.");

            if (dto.CheckIn < DateOnly.FromDateTime(DateTime.UtcNow))
                throw new ArgumentException("CheckIn date cannot be in the past.");

            // Ελέγχουμε διαθεσιμότητα
            var hasOverlap = await _bookingRepository.HasOverlapAsync(dto.RoomId, dto.CheckIn, dto.CheckOut);
            if (hasOverlap)
                throw new ArgumentException("The room is not available for the selected dates.");

            // Υπολογισμός TotalPrice
            var roomType = await _roomTypeRepository.GetByIdAsync(room.RoomTypeId);
            var nights = dto.CheckOut.DayNumber - dto.CheckIn.DayNumber;
            var totalPrice = roomType!.BasePrice * nights;

            var booking = new Booking
            {
                UserId = userId,
                RoomId = dto.RoomId,
                CheckIn = dto.CheckIn,
                CheckOut = dto.CheckOut,
                TotalPrice = totalPrice
            };

            booking.Id = await _bookingRepository.CreateAsync(booking);
            booking.Status = "Pending";
            booking.CreatedAt = DateTime.UtcNow;

            return MapToDto(booking);
        }

        public async Task CancelAsync(int bookingId, int requestingUserId, string requestingUserRole)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
                throw new KeyNotFoundException("Booking not found.");

            // Resource-level authorization
            if (requestingUserRole == "Guest" && booking.UserId != requestingUserId)
                throw new UnauthorizedAccessException("Δεν έχετε δικαίωμα να ακυρώσετε αυτό το booking.");

            if (booking.Status == "Cancelled")
                throw new ArgumentException("Booking is already cancelled.");

            if (booking.Status == "Completed")
                throw new ArgumentException("A completed booking cannot be cancelled.");

            await _bookingRepository.CancelAsync(bookingId);
        }

        private static BookingResponseDto MapToDto(Booking booking)
        {
            return new BookingResponseDto
            {
                Id = booking.Id,
                UserId = booking.UserId,
                RoomId = booking.RoomId,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                Status = booking.Status,
                TotalPrice = booking.TotalPrice,
                CreatedAt = booking.CreatedAt
            };
        }
    }
}
