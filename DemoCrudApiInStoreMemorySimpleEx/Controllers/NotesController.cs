using Microsoft.AspNetCore.Mvc;
using NoteApp.Data;
using NoteApp.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NoteApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        // GET: api/<NotesController>
        private readonly NoteStore _store;

        public NotesController(NoteStore store)
        {
            _store = store;
        }

        [HttpGet]
        public IActionResult GetAll()
        {

            var result = _store.notes.Select(n => new NoteDTO
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content
            });

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var note = _store.notes.FirstOrDefault(n => n.Id == id);

            if (note is null)
                return NotFound();

            var dto = new NoteDTO
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content
            };

            return Ok(dto);
        }

        // POST api/<NotesController>
        [HttpPost]
        public IActionResult Create (CreateNoteDTO dto)
        {
            var newId = _store.notes.Any()
            ? _store.notes.Max(n => n.Id) + 1
            : 1;
            var note = new Models.Note
            {
                Id = newId,
                Title = dto.Title,
                Content = dto.Content,
                IsArchived = false
            };
            _store.notes.Add(note);
            var result = new NoteDTO
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content
            };
           
            return Ok(result);
        }

        // PUT api/<NotesController>/5
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, UpdateNoteDTO dto)
        {
            var note = _store.notes.FirstOrDefault(n => n.Id == id);

            if (note is null)
                return NotFound();


            note.Title = dto.Title;
            note.Content = dto.Content;
            note.IsArchived = dto.IsArchived;

            var result = new NoteDTO
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content
            };

            return Ok(result);
        }

        // DELETE api/<NotesController>/5
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var note = _store.notes.FirstOrDefault(n => n.Id == id);

            if (note is null)
                return NotFound();

            _store.notes.Remove(note);

            return Ok(); 
        }
    }
}
