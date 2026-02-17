using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _service;
        public TodoController(ITodoService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoDTO>>> GetAll()
        {
            var todos = await _service.GetAllAsync();
            return Ok(todos);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTodoDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return Ok(created);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var todo = await _service.GetByIdAsync(id);

            if (todo is null)
                return NotFound();

            return Ok(todo);

        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateTodoDTO dto)
        {
            var ok = await _service.UpdateAsync(id, dto);

            if (!ok)
                return NotFound();

            return Ok();
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);

            if (!ok)
                return NotFound();

            return Ok(); 
        }
    }
}