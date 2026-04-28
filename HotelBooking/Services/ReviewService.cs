using HotelBooking.DTOs.Review;
using HotelBooking.Models;
using HotelBooking.Repositories;

namespace HotelBooking.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;

        public ReviewService(
            IReviewRepository reviewRepository,
            IBookingRepository bookingRepository,
            IRoomRepository roomRepository)
        {
            _reviewRepository = reviewRepository;
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
        }

        public async Task<IEnumerable<ReviewResponseDto>> GetByHotelIdAsync(int hotelId)
        {
            var reviews = await _reviewRepository.GetByHotelIdAsync(hotelId);
            return reviews.Select(MapToDto);
        }

        public async Task<ReviewResponseDto> CreateAsync(int requestingUserId, string requestingUserRole, CreateReviewDto dto)
        {
            // Ελέγχουμε αν υπάρχει το booking
            var booking = await _bookingRepository.GetByIdAsync(dto.BookingId);
            if (booking == null)
                throw new Exception("Το booking δεν βρέθηκε.");

            // Resource-level authorization
            if (requestingUserRole == "Guest" && booking.UserId != requestingUserId)
                throw new UnauthorizedAccessException("Δεν έχετε δικαίωμα να κάνετε review για αυτό το booking.");

            // Το booking πρέπει να είναι Completed
            if (booking.Status != "Completed")
                throw new Exception("Μπορείτε να κάνετε review μόνο για ολοκληρωμένες κρατήσεις.");

            // Ελέγχουμε αν υπάρχει ήδη review για αυτό το booking
            var existingReview = await _reviewRepository.GetByBookingIdAsync(dto.BookingId);
            if (existingReview != null)
                throw new Exception("Έχετε ήδη κάνει review για αυτό το booking.");

            // Παίρνουμε το HotelId από το Room του Booking
            var room = await _roomRepository.GetByIdAsync(booking.RoomId);

            var review = new Review
            {
                HotelId = room!.HotelId,
                UserId = requestingUserId,
                BookingId = dto.BookingId,
                Rating = dto.Rating,
                Comment = dto.Comment
            };

            review.Id = await _reviewRepository.CreateAsync(review);
            review.CreatedAt = DateTime.UtcNow;

            return MapToDto(review);
        }

        private static ReviewResponseDto MapToDto(Review review)
        {
            return new ReviewResponseDto
            {
                Id = review.Id,
                HotelId = review.HotelId,
                UserId = review.UserId,
                BookingId = review.BookingId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            };
        }
    }
}
