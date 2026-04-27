using HotelBooking.DTOs.Hotel;

namespace HotelBooking.Services
{
    public interface IHotelService
    {
        Task<IEnumerable<HotelResponseDto>> GetAllAsync(int page, int pageSize, string? city, string? country, int? starRating);
        Task<HotelResponseDto?> GetByIdAsync(int id);
        Task<IEnumerable<HotelResponseDto>> GetByOwnerIdAsync(int ownerId);
        Task<HotelResponseDto> CreateAsync(int ownerId, CreateHotelDto dto);
        Task UpdateAsync(int hotelId, int requestingUserId, string requestingUserRole, UpdateHotelDto dto);
        Task SoftDeleteAsync(int hotelId, int requestingUserId, string requestingUserRole);
    }
}