using System.ComponentModel.DataAnnotations;

namespace HotelBooking.DTOs.Booking
{
    public class CreateBookingDto
    {
        [Required(ErrorMessage = "Το RoomId είναι υποχρεωτικό")]
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Το CheckIn είναι υποχρεωτικό")]
        public DateOnly CheckIn { get; set; }

        [Required(ErrorMessage = "Το CheckOut είναι υποχρεωτικό")]
        public DateOnly CheckOut { get; set; }
    }
}
