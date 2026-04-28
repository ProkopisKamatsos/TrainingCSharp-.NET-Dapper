using System.ComponentModel.DataAnnotations;

namespace HotelBooking.DTOs.RoomType
{
    public class CreateRoomTypeDto
    {
        [Required(ErrorMessage = "Το HotelId είναι υποχρεωτικό")]
        public int HotelId { get; set; }

        [Required(ErrorMessage = "Το Name είναι υποχρεωτικό")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Το Name πρέπει να είναι μεταξύ 2 και 50 χαρακτήρων")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Η Description δεν μπορεί να ξεπερνά 500 χαρακτήρες")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Το BasePrice είναι υποχρεωτικό")]
        [Range(1, 10000, ErrorMessage = "Το BasePrice πρέπει να είναι μεταξύ 1 και 10000")]
        public decimal BasePrice { get; set; }

        [Required(ErrorMessage = "Το Capacity είναι υποχρεωτικό")]
        [Range(1, 10, ErrorMessage = "Το Capacity πρέπει να είναι μεταξύ 1 και 10")]
        public int Capacity { get; set; }

        public List<string> Amenities { get; set; } = new();
    }
}