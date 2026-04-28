using HotelBooking.DTOs.RoomType;

namespace HotelBooking.Services
{
    public interface IRoomTypeService
    {
        Task<IEnumerable<RoomTypeResponseDto>> GetByHotelIdAsync(int hotelId);
        Task<RoomTypeResponseDto> GetByIdAsync(int id);
        Task<RoomTypeResponseDto> CreateAsync(int requestingUserId, string requestingUserRole, CreateRoomTypeDto dto);
        Task UpdateAsync(int roomTypeId, int requestingUserId, string requestingUserRole, UpdateRoomTypeDto dto);
        Task DeleteAsync(int roomTypeId, int requestingUserId, string requestingUserRole);
    }
}