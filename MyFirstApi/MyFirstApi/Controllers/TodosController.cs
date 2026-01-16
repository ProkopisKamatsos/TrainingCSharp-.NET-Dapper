using Microsoft.AspNetCore.Mvc;
using MyFirstApi.Models;

namespace MyFirstApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private static readonly List<TodoItem> Todos =
    [
        new TodoItem { Id = 1, Title = "Learn C#", IsDone = false },
        new TodoItem { Id = 2, Title = "Build Web API", IsDone = false }
    ];

    [HttpGet]
    public ActionResult<List<TodoItem>> GetAll()
    {
        return Ok(Todos);
    }

    [HttpGet("{id:int}")]
    public ActionResult<TodoItem> GetById(int id)
    {
        var todo = Todos.FirstOrDefault(t => t.Id == id);
        if (todo is null) return NotFound();
        return Ok(todo);
    }

    [HttpPost]
    public ActionResult<TodoItem> Create([FromBody] TodoItem input)
    {
        var nextId = Todos.Count == 0 ? 1 : Todos.Max(t => t.Id) + 1;

        var todo = new TodoItem
        {
            Id = nextId,
            Title = input.Title,
            IsDone = input.IsDone
        };

        Todos.Add(todo);

        return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] TodoItem input)
    {
        var todo = Todos.FirstOrDefault(t => t.Id == id);
        if (todo is null) return NotFound();

        todo.Title = input.Title;
        todo.IsDone = input.IsDone;

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var todo = Todos.FirstOrDefault(t => t.Id == id);
        if (todo is null) return NotFound();

        Todos.Remove(todo);
        return NoContent();
    }
}
