using System.ComponentModel.DataAnnotations;

namespace HotelBooking.DTOs.Review
{
    public class CreateReviewDto
    {
        [Required(ErrorMessage = "Το BookingId είναι υποχρεωτικό")]
        public int BookingId { get; set; }

        [Required(ErrorMessage = "Το Rating είναι υποχρεωτικό")]
        [Range(1, 5, ErrorMessage = "Το Rating πρέπει να είναι μεταξύ 1 και 5")]
        public int Rating { get; set; }

        [StringLength(1000, ErrorMessage = "Το Comment δεν μπορεί να ξεπερνά 1000 χαρακτήρες")]
        public string? Comment { get; set; }
    }
}