using System.ComponentModel.DataAnnotations;

namespace HotelBooking.DTOs.Auth
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Το FullName είναι υποχρεωτικό")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Το FullName πρέπει να είναι μεταξύ 2 και 100 χαρακτήρων")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Το Email είναι υποχρεωτικό")]
        [EmailAddress(ErrorMessage = "Μη έγκυρο Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Το Password είναι υποχρεωτικό")]
        [MinLength(8, ErrorMessage = "Το Password πρέπει να έχει τουλάχιστον 8 χαρακτήρες")]
        public string Password { get; set; } = string.Empty;
    }
}