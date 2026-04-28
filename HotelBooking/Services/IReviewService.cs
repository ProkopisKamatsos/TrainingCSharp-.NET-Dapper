using HotelBooking.DTOs.Review;

namespace HotelBooking.Services
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewResponseDto>> GetByHotelIdAsync(int hotelId);
        Task<ReviewResponseDto> CreateAsync(int requestingUserId, string requestingUserRole, CreateReviewDto dto);
    }
}