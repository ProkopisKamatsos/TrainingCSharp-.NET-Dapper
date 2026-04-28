using HotelBooking.DTOs.Hotel;
using HotelBooking.Models;
using HotelBooking.Repositories;

namespace HotelBooking.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;

        public HotelService(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task<IEnumerable<HotelResponseDto>> GetAllAsync(int page, int pageSize, string? city, string? country, int? starRating)
        {
            var hotels = await _hotelRepository.GetAllAsync(page, pageSize, city, country, starRating);
            return hotels.Select(MapToDto);
        }

        public async Task<HotelResponseDto> GetByIdAsync(int id)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id);
            if (hotel == null)
                throw new KeyNotFoundException("Hotel not found.");
            return MapToDto(hotel);
        }

        public async Task<IEnumerable<HotelResponseDto>> GetByOwnerIdAsync(int ownerId)
        {
            var hotels = await _hotelRepository.GetByOwnerIdAsync(ownerId);
            return hotels.Select(MapToDto);
        }

        public async Task<HotelResponseDto> CreateAsync(int ownerId, CreateHotelDto dto)
        {
            var hotel = new Hotel
            {
                OwnerId = ownerId,
                Name = dto.Name,
                Description = dto.Description,
                Address = dto.Address,
                City = dto.City,
                Country = dto.Country,
                StarRating = dto.StarRating,
                Amenities = dto.Amenities
                    .Select(a => new HotelAmenity { Name = a })
                    .ToList()
            };

            hotel.Id = await _hotelRepository.CreateAsync(hotel);

            // Φέρνουμε το hotel από τη βάση για να έχουμε το σωστό CreatedAt
            var created = await _hotelRepository.GetByIdAsync(hotel.Id);
            return MapToDto(created!);
        }

        public async Task UpdateAsync(int hotelId, int requestingUserId, string requestingUserRole, UpdateHotelDto dto)
        {
            var hotel = await _hotelRepository.GetByIdAsync(hotelId);
            if (hotel == null)
                throw new KeyNotFoundException("Hotel not found.");

            // Resource-level authorization
            if (requestingUserRole == "HotelManager" && hotel.OwnerId != requestingUserId)
                throw new UnauthorizedAccessException("Δεν έχετε δικαίωμα να επεξεργαστείτε αυτό το hotel.");

            hotel.Name = dto.Name;
            hotel.Description = dto.Description;
            hotel.Address = dto.Address;
            hotel.City = dto.City;
            hotel.Country = dto.Country;
            hotel.StarRating = dto.StarRating;
            hotel.Amenities = dto.Amenities
                .Select(a => new HotelAmenity { Name = a })
                .ToList();

            await _hotelRepository.UpdateAsync(hotel);
        }

        public async Task SoftDeleteAsync(int hotelId, int requestingUserId, string requestingUserRole)
        {
            var hotel = await _hotelRepository.GetByIdAsync(hotelId);
            if (hotel == null)
                throw new KeyNotFoundException("Hotel not found.");

            // Resource-level authorization
            if (requestingUserRole == "HotelManager" && hotel.OwnerId != requestingUserId)
                throw new UnauthorizedAccessException("Δεν έχετε δικαίωμα να διαγράψετε αυτό το hotel.");

            await _hotelRepository.SoftDeleteAsync(hotelId);
        }

        // Private helper - mapping Hotel → HotelResponseDto
        private static HotelResponseDto MapToDto(Hotel hotel)
        {
            return new HotelResponseDto
            {
                Id = hotel.Id,
                OwnerId = hotel.OwnerId,
                Name = hotel.Name,
                Description = hotel.Description,
                Address = hotel.Address,
                City = hotel.City,
                Country = hotel.Country,
                StarRating = hotel.StarRating,
                CreatedAt = hotel.CreatedAt,
                Amenities = hotel.Amenities.Select(a => a.Name).ToList()
            };
        }
    }
}