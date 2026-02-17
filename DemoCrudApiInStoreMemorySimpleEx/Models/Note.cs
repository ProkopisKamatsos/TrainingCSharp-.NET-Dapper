namespace NoteApp.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Content { get; set; }
        public bool IsArchived { get; set; }
    }
}
