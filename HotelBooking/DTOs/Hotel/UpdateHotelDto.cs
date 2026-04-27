using System.ComponentModel.DataAnnotations;

namespace HotelBooking.DTOs.Hotel
{
    public class UpdateHotelDto
    {
        [Required(ErrorMessage = "Το Name είναι υποχρεωτικό")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Το Name πρέπει να είναι μεταξύ 3 και 150 χαρακτήρων")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Η Description δεν μπορεί να ξεπερνά 1000 χαρακτήρες")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Το Address είναι υποχρεωτικό")]
        [StringLength(200, ErrorMessage = "Το Address δεν μπορεί να ξεπερνά 200 χαρακτήρες")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Το City είναι υποχρεωτικό")]
        [StringLength(100, ErrorMessage = "Το City δεν μπορεί να ξεπερνά 100 χαρακτήρες")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Το Country είναι υποχρεωτικό")]
        [StringLength(100, ErrorMessage = "Το Country δεν μπορεί να ξεπερνά 100 χαρακτήρες")]
        public string Country { get; set; } = string.Empty;

        [Range(1, 5, ErrorMessage = "Το StarRating πρέπει να είναι μεταξύ 1 και 5")]
        public int StarRating { get; set; }

        public List<string> Amenities { get; set; } = new();
    }
}