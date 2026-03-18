using EmployeeAPIviaDapper.Models;
using EmployeeAPIviaDapper.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAPIviaDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;
        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _service.GetEmployees();
            if (employees == null)
            {
                return NotFound();
            }
            return Ok(employees);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeesById(int id)
        {
            var employees = await _service.GetEmployeeById(id);
            if (employees == null)
            {
                return NotFound();
            }
            return Ok(employees);
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest();
            }
            var result = await _service.CreateEmployee(employee);
            if (result == 0)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest();
            }
            var result = await _service.UpdateEmployee(id, employee);
            if (result == 0)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var result = await _service.DeleteEmployee(id);
            if (result == 0)
            {
                return BadRequest();
            }
            return Ok(result);
        }

    }
}
