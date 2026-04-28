using System.Security.Claims;
using HotelBooking.DTOs.Room;
using HotelBooking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        // GET api/rooms/hotel/1
        [HttpGet("hotel/{hotelId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByHotelId(int hotelId)
        {
            var rooms = await _roomService.GetByHotelIdAsync(hotelId);
            return Ok(rooms);
        }

        // GET api/rooms/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var room = await _roomService.GetByIdAsync(id);
            if (room == null)
                return NotFound(new { message = "Το δωμάτιο δεν βρέθηκε." });

            return Ok(room);
        }

        // GET api/rooms/available?hotelId=1&checkIn=2026-06-01&checkOut=2026-06-05
        [HttpGet("available")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvailableRooms(
            [FromQuery] int hotelId,
            [FromQuery] DateOnly checkIn,
            [FromQuery] DateOnly checkOut)
        {
            try
            {
                var rooms = await _roomService.GetAvailableRoomsAsync(hotelId, checkIn, checkOut);
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST api/rooms
        [HttpPost]
        [Authorize(Roles = "Admin,HotelManager")]
        public async Task<IActionResult> Create([FromBody] CreateRoomDto dto)
        {
            try
            {
                var requestingUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var requestingUserRole = User.FindFirstValue(ClaimTypes.Role)!;

                var room = await _roomService.CreateAsync(requestingUserId, requestingUserRole, dto);
                return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT api/rooms/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,HotelManager")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoomDto dto)
        {
            try
            {
                var requestingUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var requestingUserRole = User.FindFirstValue(ClaimTypes.Role)!;

                await _roomService.UpdateAsync(id, requestingUserId, requestingUserRole, dto);
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE api/rooms/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,HotelManager")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            try
            {
                var requestingUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var requestingUserRole = User.FindFirstValue(ClaimTypes.Role)!;

                await _roomService.SoftDeleteAsync(id, requestingUserId, requestingUserRole);
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}