namespace HotelBooking.DTOs.Room
{
    public class RoomResponseDto
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public int RoomTypeId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public int Floor { get; set; }
    }
}