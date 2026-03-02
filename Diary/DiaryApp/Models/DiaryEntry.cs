using System.ComponentModel.DataAnnotations;

namespace DiaryApp.Models
{
    public class DiaryEntry
    {
        // [Key]
        public int Id { get; set; }
        [Required]
        //Client side validation
        // [StringLength(100, MinimumLength = 3,
        // ErrorMessage = "The tile must be over 3 chars and below 100")]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
        [Required]
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
