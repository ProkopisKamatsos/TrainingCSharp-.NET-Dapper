using System.Security.Claims;
using HotelBooking.DTOs.RoomType;
using HotelBooking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoomTypesController : ControllerBase
    {
        private readonly IRoomTypeService _roomTypeService;

        public RoomTypesController(IRoomTypeService roomTypeService)
        {
            _roomTypeService = roomTypeService;
        }

        // GET api/roomtypes/hotel/1
        [HttpGet("hotel/{hotelId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByHotelId(int hotelId)
        {
            var roomTypes = await _roomTypeService.GetByHotelIdAsync(hotelId);
            return Ok(roomTypes);
        }

        // GET api/roomtypes/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var roomType = await _roomTypeService.GetByIdAsync(id);
            if (roomType == null)
                return NotFound(new { message = "Το RoomType δεν βρέθηκε." });

            return Ok(roomType);
        }

        // POST api/roomtypes
        [HttpPost]
        [Authorize(Roles = "Admin,HotelManager")]
        public async Task<IActionResult> Create([FromBody] CreateRoomTypeDto dto)
        {
            try
            {
                var requestingUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var requestingUserRole = User.FindFirstValue(ClaimTypes.Role)!;

                var roomType = await _roomTypeService.CreateAsync(requestingUserId, requestingUserRole, dto);
                return CreatedAtAction(nameof(GetById), new { id = roomType.Id }, roomType);
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

        // PUT api/roomtypes/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,HotelManager")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoomTypeDto dto)
        {
            try
            {
                var requestingUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var requestingUserRole = User.FindFirstValue(ClaimTypes.Role)!;

                await _roomTypeService.UpdateAsync(id, requestingUserId, requestingUserRole, dto);
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

        // DELETE api/roomtypes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,HotelManager")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var requestingUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var requestingUserRole = User.FindFirstValue(ClaimTypes.Role)!;

                await _roomTypeService.DeleteAsync(id, requestingUserId, requestingUserRole);
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