using NoteApp.Models;
namespace NoteApp.Data
{
    public class NoteStore
    {
       public List<Note> notes = new List<Note>();
       
        
            public NoteStore()
        {
            notes.Add(new Note { Id = 1, Title = "First note", Content = "hello", IsArchived = false });
            notes.Add(new Note { Id = 2, Title = "Second note", Content = null, IsArchived = false });
        }

    }

}

