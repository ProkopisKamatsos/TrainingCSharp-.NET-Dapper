using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsCrudMvc.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Το όνομα είναι υποχρεωτικό")]
    [StringLength(100)]
    [Display(Name = "Όνομα")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    [Display(Name = "Περιγραφή")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Η τιμή είναι υποχρεωτική")]
    [Range(0.01, 99999.99, ErrorMessage = "Η τιμή πρέπει να είναι μεταξύ 0.01 και 99999.99")]
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Τιμή (€)")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Η κατηγορία είναι υποχρεωτική")]
    [StringLength(50)]
    [Display(Name = "Κατηγορία")]
    public string Category { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "Το απόθεμα δεν μπορεί να είναι αρνητικό")]
    [Display(Name = "Απόθεμα")]
    public int Stock { get; set; }

    [Display(Name = "Ημ/νία Δημιουργίας")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
