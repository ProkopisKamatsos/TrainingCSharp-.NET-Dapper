using HotelBooking.DTOs.Room;
using HotelBooking.Models;
using HotelBooking.Repositories;

namespace HotelBooking.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IHotelRepository _hotelRepository;

        public RoomService(IRoomRepository roomRepository, IHotelRepository hotelRepository)
        {
            _roomRepository = roomRepository;
            _hotelRepository = hotelRepository;
        }

        public async Task<IEnumerable<RoomResponseDto>> GetByHotelIdAsync(int hotelId)
        {
            var rooms = await _roomRepository.GetByHotelIdAsync(hotelId);
            return rooms.Select(MapToDto);
        }

        public async Task<RoomResponseDto> GetByIdAsync(int id)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null)
                throw new KeyNotFoundException("Room not found.");
            return MapToDto(room);
        }

        public async Task<IEnumerable<RoomResponseDto>> GetAvailableRoomsAsync(int hotelId, DateOnly checkIn, DateOnly checkOut)
        {
            if (checkIn >= checkOut)
                throw new ArgumentException("CheckIn date must be before CheckOut date.");

            if (checkIn < DateOnly.FromDateTime(DateTime.UtcNow))
                throw new ArgumentException("CheckIn date cannot be in the past.");

            var rooms = await _roomRepository.GetAvailableRoomsAsync(hotelId, checkIn, checkOut);
            return rooms.Select(MapToDto);
        }

        public async Task<RoomResponseDto> CreateAsync(int requestingUserId, string requestingUserRole, CreateRoomDto dto)
        {
            // Ελέγχουμε αν υπάρχει το hotel
            var hotel = await _hotelRepository.GetByIdAsync(dto.HotelId);
            if (hotel == null)
                throw new KeyNotFoundException("Hotel not found.");

            // Resource-level authorization
            if (requestingUserRole == "HotelManager" && hotel.OwnerId != requestingUserId)
                throw new UnauthorizedAccessException("Δεν έχετε δικαίωμα να προσθέσετε δωμάτιο σε αυτό το hotel.");

            var room = new Room
            {
                HotelId = dto.HotelId,
                RoomTypeId = dto.RoomTypeId,
                RoomNumber = dto.RoomNumber,
                Floor = dto.Floor
            };

            room.Id = await _roomRepository.CreateAsync(room);
            return MapToDto(room);
        }

        public async Task UpdateAsync(int roomId, int requestingUserId, string requestingUserRole, UpdateRoomDto dto)
        {
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null)
                throw new KeyNotFoundException("Room not found.");

            // Φέρνουμε το hotel για να ελέγξουμε ownership
            var hotel = await _hotelRepository.GetByIdAsync(room.HotelId);

            // Resource-level authorization
            if (requestingUserRole == "HotelManager" && hotel!.OwnerId != requestingUserId)
                throw new UnauthorizedAccessException("Δεν έχετε δικαίωμα να επεξεργαστείτε αυτό το δωμάτιο.");

            room.RoomTypeId = dto.RoomTypeId;
            room.RoomNumber = dto.RoomNumber;
            room.Floor = dto.Floor;

            await _roomRepository.UpdateAsync(room);
        }

        public async Task SoftDeleteAsync(int roomId, int requestingUserId, string requestingUserRole)
        {
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null)
                throw new KeyNotFoundException("Room not found.");

            // Φέρνουμε το hotel για να ελέγξουμε ownership
            var hotel = await _hotelRepository.GetByIdAsync(room.HotelId);

            // Resource-level authorization
            if (requestingUserRole == "HotelManager" && hotel!.OwnerId != requestingUserId)
                throw new UnauthorizedAccessException("Δεν έχετε δικαίωμα να διαγράψετε αυτό το δωμάτιο.");

            await _roomRepository.SoftDeleteAsync(roomId);
        }

        private static RoomResponseDto MapToDto(Room room)
        {
            return new RoomResponseDto
            {
                Id = room.Id,
                HotelId = room.HotelId,
                RoomTypeId = room.RoomTypeId,
                RoomNumber = room.RoomNumber,
                Floor = room.Floor
            };
        }
    }
}