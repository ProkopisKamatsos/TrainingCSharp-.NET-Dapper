using System.ComponentModel.DataAnnotations;

namespace HotelBooking.DTOs.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Το Email είναι υποχρεωτικό")]
        [EmailAddress(ErrorMessage = "Μη έγκυρο Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Το Password είναι υποχρεωτικό")]
        public string Password { get; set; } = string.Empty;
    }
}