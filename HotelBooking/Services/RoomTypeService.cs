using HotelBooking.DTOs.RoomType;
using HotelBooking.Models;
using HotelBooking.Repositories;

namespace HotelBooking.Services
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IHotelRepository _hotelRepository;

        public RoomTypeService(IRoomTypeRepository roomTypeRepository, IHotelRepository hotelRepository)
        {
            _roomTypeRepository = roomTypeRepository;
            _hotelRepository = hotelRepository;
        }

        public async Task<IEnumerable<RoomTypeResponseDto>> GetByHotelIdAsync(int hotelId)
        {
            var roomTypes = await _roomTypeRepository.GetByHotelIdAsync(hotelId);
            return roomTypes.Select(MapToDto);
        }

        public async Task<RoomTypeResponseDto> GetByIdAsync(int id)
        {
            var roomType = await _roomTypeRepository.GetByIdAsync(id);
            if (roomType == null)
                throw new KeyNotFoundException("RoomType not found.");
            return MapToDto(roomType);
        }

        public async Task<RoomTypeResponseDto> CreateAsync(int requestingUserId, string requestingUserRole, CreateRoomTypeDto dto)
        {
            // Ελέγχουμε αν υπάρχει το hotel
            var hotel = await _hotelRepository.GetByIdAsync(dto.HotelId);
            if (hotel == null)
                throw new KeyNotFoundException("Hotel not found.");

            // Resource-level authorization
            if (requestingUserRole == "HotelManager" && hotel.OwnerId != requestingUserId)
                throw new UnauthorizedAccessException("Δεν έχετε δικαίωμα να προσθέσετε RoomType σε αυτό το hotel.");

            var roomType = new RoomType
            {
                HotelId = dto.HotelId,
                Name = dto.Name,
                Description = dto.Description,
                BasePrice = dto.BasePrice,
                Capacity = dto.Capacity,
                Amenities = dto.Amenities
                    .Select(a => new RoomTypeAmenity { Name = a })
                    .ToList()
            };

            roomType.Id = await _roomTypeRepository.CreateAsync(roomType);
            return MapToDto(roomType);
        }

        public async Task UpdateAsync(int roomTypeId, int requestingUserId, string requestingUserRole, UpdateRoomTypeDto dto)
        {
            var roomType = await _roomTypeRepository.GetByIdAsync(roomTypeId);
            if (roomType == null)
                throw new KeyNotFoundException("RoomType not found.");

            // Φέρνουμε το hotel για να ελέγξουμε ownership
            var hotel = await _hotelRepository.GetByIdAsync(roomType.HotelId);

            // Resource-level authorization
            if (requestingUserRole == "HotelManager" && hotel!.OwnerId != requestingUserId)
                throw new UnauthorizedAccessException("Δεν έχετε δικαίωμα να επεξεργαστείτε αυτό το RoomType.");

            roomType.Name = dto.Name;
            roomType.Description = dto.Description;
            roomType.BasePrice = dto.BasePrice;
            roomType.Capacity = dto.Capacity;
            roomType.Amenities = dto.Amenities
                .Select(a => new RoomTypeAmenity { Name = a })
                .ToList();

            await _roomTypeRepository.UpdateAsync(roomType);
        }

        public async Task DeleteAsync(int roomTypeId, int requestingUserId, string requestingUserRole)
        {
            var roomType = await _roomTypeRepository.GetByIdAsync(roomTypeId);
            if (roomType == null)
                throw new KeyNotFoundException("RoomType not found.");

            // Φέρνουμε το hotel για να ελέγξουμε ownership
            var hotel = await _hotelRepository.GetByIdAsync(roomType.HotelId);

            // Resource-level authorization
            if (requestingUserRole == "HotelManager" && hotel!.OwnerId != requestingUserId)
                throw new UnauthorizedAccessException("Δεν έχετε δικαίωμα να διαγράψετε αυτό το RoomType.");

            await _roomTypeRepository.DeleteAsync(roomTypeId);
        }

        private static RoomTypeResponseDto MapToDto(RoomType roomType)
        {
            return new RoomTypeResponseDto
            {
                Id = roomType.Id,
                HotelId = roomType.HotelId,
                Name = roomType.Name,
                Description = roomType.Description,
                BasePrice = roomType.BasePrice,
                Capacity = roomType.Capacity,
                Amenities = roomType.Amenities.Select(a => a.Name).ToList()
            };
        }
    }
}
