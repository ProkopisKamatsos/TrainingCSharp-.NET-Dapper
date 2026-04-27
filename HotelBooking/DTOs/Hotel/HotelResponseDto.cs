namespace HotelBooking.DTOs.Hotel
{
    public class HotelResponseDto
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public int StarRating { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> Amenities { get; set; } = new();
    }
}