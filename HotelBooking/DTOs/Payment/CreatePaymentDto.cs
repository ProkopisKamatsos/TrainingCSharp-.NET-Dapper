using System.ComponentModel.DataAnnotations;

namespace HotelBooking.DTOs.Payment
{
    public class CreatePaymentDto
    {
        [Required(ErrorMessage = "Το BookingId είναι υποχρεωτικό")]
        public int BookingId { get; set; }

        [Required(ErrorMessage = "Η μέθοδος πληρωμής είναι υποχρεωτική")]
        [RegularExpression("^(CreditCard|DebitCard|PayPal|Cash)$",
            ErrorMessage = "Η μέθοδος πληρωμής πρέπει να είναι CreditCard, DebitCard, PayPal ή Cash")]
        public string Method { get; set; } = string.Empty;
    }
}