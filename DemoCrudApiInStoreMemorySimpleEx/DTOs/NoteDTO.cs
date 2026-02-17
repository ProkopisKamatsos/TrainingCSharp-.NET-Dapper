using System.ComponentModel.DataAnnotations;

namespace NoteApp.DTOs
{
    public class NoteDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;
        [StringLength(1000)]
        public string? Content { get; set; }
    }
}
