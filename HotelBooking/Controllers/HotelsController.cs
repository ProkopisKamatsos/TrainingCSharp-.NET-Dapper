using System.Security.Claims;
using HotelBooking.DTOs.Hotel;
using HotelBooking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelsController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        // GET api/hotels?page=1&pageSize=10&city=Athens&country=Greece&starRating=5
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? city = null,
            [FromQuery] string? country = null,
            [FromQuery] int? starRating = null)
        {
            var hotels = await _hotelService.GetAllAsync(page, pageSize, city, country, starRating);
            return Ok(hotels);
        }

        // GET api/hotels/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var hotel = await _hotelService.GetByIdAsync(id);
            return Ok(hotel);
        }

        // GET api/hotels/my
        [HttpGet("my")]
        [Authorize(Roles = "HotelManager")]
        public async Task<IActionResult> GetMyHotels()
        {
            var ownerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var hotels = await _hotelService.GetByOwnerIdAsync(ownerId);
            return Ok(hotels);
        }

        // POST api/hotels
        [HttpPost]
        [Authorize(Roles = "Admin,HotelManager")]
        public async Task<IActionResult> Create([FromBody] CreateHotelDto dto)
        {
            var ownerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var hotel = await _hotelService.CreateAsync(ownerId, dto);
            return CreatedAtAction(nameof(GetById), new { id = hotel.Id }, hotel);
        }

        // PUT api/hotels/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,HotelManager")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateHotelDto dto)
        {
            var requestingUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var requestingUserRole = User.FindFirstValue(ClaimTypes.Role)!;

            await _hotelService.UpdateAsync(id, requestingUserId, requestingUserRole, dto);
            return NoContent();
        }

        // DELETE api/hotels/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,HotelManager")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var requestingUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var requestingUserRole = User.FindFirstValue(ClaimTypes.Role)!;

            await _hotelService.SoftDeleteAsync(id, requestingUserId, requestingUserRole);
            return NoContent();
        }
    }
}
