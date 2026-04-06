using System.ComponentModel.DataAnnotations;

namespace ProductsCrudMvc.Models.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Το email είναι υποχρεωτικό")]
    [EmailAddress(ErrorMessage = "Μη έγκυρο email")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ο κωδικός είναι υποχρεωτικός")]
    [DataType(DataType.Password)]
    [Display(Name = "Κωδικός")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Να με θυμάσαι")]
    public bool RememberMe { get; set; }
}
