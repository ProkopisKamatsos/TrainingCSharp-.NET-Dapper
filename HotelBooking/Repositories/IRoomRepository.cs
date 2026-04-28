using HotelBooking.Models;

namespace HotelBooking.Repositories
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetByHotelIdAsync(int hotelId);
        Task<Room?> GetByIdAsync(int id);
        Task<IEnumerable<Room>> GetAvailableRoomsAsync(int hotelId, DateOnly checkIn, DateOnly checkOut);
        Task<int> CreateAsync(Room room);
        Task UpdateAsync(Room room);
        Task SoftDeleteAsync(int id);
    }
}