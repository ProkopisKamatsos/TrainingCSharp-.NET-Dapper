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
            var requestingUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var requestingUserRole = User.FindFirstValue(ClaimTypes.Role)!;

            var booking = await _bookingService.GetByIdAsync(id, requestingUserId, requestingUserRole);
            return Ok(booking);
        }

        // POST api/bookings
        [HttpPost]
        [Authorize(Roles = "Admin,Guest")]
        public async Task<IActionResult> Create([FromBody] CreateBookingDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var booking = await _bookingService.CreateAsync(userId, dto);
            return CreatedAtAction(nameof(GetById), new { id = booking.Id }, booking);
        }

        // PUT api/bookings/5/cancel
        [HttpPut("{id}/cancel")]
        [Authorize(Roles = "Admin,Guest")]
        public async Task<IActionResult> Cancel(int id)
        {
            var requestingUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var requestingUserRole = User.FindFirstValue(ClaimTypes.Role)!;

            await _bookingService.CancelAsync(id, requestingUserId, requestingUserRole);
            return NoContent();
        }
    }
}
