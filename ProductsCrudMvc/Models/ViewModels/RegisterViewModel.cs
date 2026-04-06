using System.ComponentModel.DataAnnotations;

namespace ProductsCrudMvc.Models.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Το email είναι υποχρεωτικό")]
    [EmailAddress(ErrorMessage = "Μη έγκυρο email")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ο κωδικός είναι υποχρεωτικός")]
    [StringLength(100, ErrorMessage = "Ο κωδικός πρέπει να έχει τουλάχιστον {2} χαρακτήρες.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Κωδικός")]
    public string Password { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Επιβεβαίωση Κωδικού")]
    [Compare("Password", ErrorMessage = "Οι κωδικοί δεν ταιριάζουν.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Display(Name = "Ρόλος")]
    public string Role { get; set; } = "User";
}
