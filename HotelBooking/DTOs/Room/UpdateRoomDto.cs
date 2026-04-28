using System.ComponentModel.DataAnnotations;

namespace HotelBooking.DTOs.Room
{
    public class UpdateRoomDto
    {
        [Required(ErrorMessage = "Το RoomTypeId είναι υποχρεωτικό")]
        public int RoomTypeId { get; set; }

        [Required(ErrorMessage = "Το RoomNumber είναι υποχρεωτικό")]
        [StringLength(10, ErrorMessage = "Το RoomNumber δεν μπορεί να ξεπερνά 10 χαρακτήρες")]
        public string RoomNumber { get; set; } = string.Empty;

        [Range(1, 100, ErrorMessage = "Ο Floor πρέπει να είναι μεταξύ 1 και 100")]
        public int Floor { get; set; }
    }
}