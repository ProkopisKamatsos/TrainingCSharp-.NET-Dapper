using Microsoft.AspNetCore.Mvc;

namespace EmployeeAPIviaDapper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet("message")]
        public IActionResult GetMessage()
        {
            return Ok("Welcome to the Employee API using Dapper!");
        }
    }
}
