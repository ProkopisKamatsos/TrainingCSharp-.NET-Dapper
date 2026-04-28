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

        public async Task<BookingResponseDto?> GetByIdAsync(int bookingId, int requestingUserId, string requestingUserRole)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null) return null;

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
                throw new Exception("Το δωμάτιο δεν βρέθηκε.");

            // Ελέγχουμε τις ημερομηνίες
            if (dto.CheckIn >= dto.CheckOut)
                throw new Exception("Η ημερομηνία CheckIn πρέπει να είναι πριν το CheckOut.");

            if (dto.CheckIn < DateOnly.FromDateTime(DateTime.UtcNow))
                throw new Exception("Η ημερομηνία CheckIn δεν μπορεί να είναι στο παρελθόν.");

            // Ελέγχουμε διαθεσιμότητα
            var hasOverlap = await _bookingRepository.HasOverlapAsync(dto.RoomId, dto.CheckIn, dto.CheckOut);
            if (hasOverlap)
                throw new Exception("Το δωμάτιο δεν είναι διαθέσιμο για τις συγκεκριμένες ημερομηνίες.");

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
                throw new Exception("Το booking δεν βρέθηκε.");

            // Resource-level authorization
            if (requestingUserRole == "Guest" && booking.UserId != requestingUserId)
                throw new UnauthorizedAccessException("Δεν έχετε δικαίωμα να ακυρώσετε αυτό το booking.");

            if (booking.Status == "Cancelled")
                throw new Exception("Το booking είναι ήδη ακυρωμένο.");

            if (booking.Status == "Completed")
                throw new Exception("Δεν μπορείτε να ακυρώσετε ένα ολοκληρωμένο booking.");

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
