using System.Security.Claims;
using HotelBooking.DTOs.Booking;
using HotelBooking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // GET api/bookings/my
        [HttpGet("my")]
        [Authorize(Roles = "Guest")]
        public async Task<IActionResult> GetMyBookings()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var bookings = await _bookingService.GetByUserIdAsync(userId);
            return Ok(bookings);
        }

        // GET api/bookings/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Guest")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var requestingUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var requestingUserRole = User.FindFirstValue(ClaimTypes.Role)!;

                var booking = await _bookingService.GetByIdAsync(id, requestingUserId, requestingUserRole);
                if (booking == null)
                    return NotFound(new { message = "Το booking δεν βρέθηκε." });

                return Ok(booking);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        // POST api/bookings
        [HttpPost]
        [Authorize(Roles = "Admin,Guest")]
        public async Task<IActionResult> Create([FromBody] CreateBookingDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var booking = await _bookingService.CreateAsync(userId, dto);
                return CreatedAtAction(nameof(GetById), new { id = booking.Id }, booking);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT api/bookings/5/cancel
        [HttpPut("{id}/cancel")]
        [Authorize(Roles = "Admin,Guest")]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                var requestingUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var requestingUserRole = User.FindFirstValue(ClaimTypes.Role)!;

                await _bookingService.CancelAsync(id, requestingUserId, requestingUserRole);
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