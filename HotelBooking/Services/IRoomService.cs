using HotelBooking.DTOs.Room;

namespace HotelBooking.Services
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomResponseDto>> GetByHotelIdAsync(int hotelId);
        Task<RoomResponseDto?> GetByIdAsync(int id);
        Task<IEnumerable<RoomResponseDto>> GetAvailableRoomsAsync(int hotelId, DateOnly checkIn, DateOnly checkOut);
        Task<RoomResponseDto> CreateAsync(int requestingUserId, string requestingUserRole, CreateRoomDto dto);
        Task UpdateAsync(int roomId, int requestingUserId, string requestingUserRole, UpdateRoomDto dto);
        Task SoftDeleteAsync(int roomId, int requestingUserId, string requestingUserRole);
    }
}