using HotelBooking.Models;

namespace HotelBooking.Repositories
{
    public interface IRoomTypeRepository
    {
        Task<IEnumerable<RoomType>> GetByHotelIdAsync(int hotelId);
        Task<RoomType?> GetByIdAsync(int id);
        Task<int> CreateAsync(RoomType roomType);
        Task UpdateAsync(RoomType roomType);
        Task DeleteAsync(int id);
    }
}