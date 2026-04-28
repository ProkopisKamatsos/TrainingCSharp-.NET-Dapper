namespace HotelBooking.DTOs.Review
{
    public class ReviewResponseDto
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public int UserId { get; set; }
        public int BookingId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}