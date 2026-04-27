using HotelBooking.Models;

namespace HotelBooking.Repositories
{
    public interface IHotelRepository
    {
        Task<IEnumerable<Hotel>> GetAllAsync(int page, int pageSize, string? city, string? country, int? starRating);
        Task<Hotel?> GetByIdAsync(int id);
        Task<IEnumerable<Hotel>> GetByOwnerIdAsync(int ownerId);
        Task<int> CreateAsync(Hotel hotel);
        Task UpdateAsync(Hotel hotel);
        Task SoftDeleteAsync(int id);
    }
}