using System.Security.Claims;
using HotelBooking.DTOs.Payment;
using HotelBooking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // GET api/payments/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Guest")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var requestingUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var requestingUserRole = User.FindFirstValue(ClaimTypes.Role)!;

                var payment = await _paymentService.GetByIdAsync(id, requestingUserId, requestingUserRole);
                if (payment == null)
                    return NotFound(new { message = "Το payment δεν βρέθηκε." });

                return Ok(payment);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        // GET api/payments/booking/1
        [HttpGet("booking/{bookingId}")]
        [Authorize(Roles = "Admin,Guest")]
        public async Task<IActionResult> GetByBookingId(int bookingId)
        {
            try
            {
                var requestingUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var requestingUserRole = User.FindFirstValue(ClaimTypes.Role)!;

                var payment = await _paymentService.GetByBookingIdAsync(bookingId, requestingUserId, requestingUserRole);
                if (payment == null)
                    return NotFound(new { message = "Το payment δεν βρέθηκε." });

                return Ok(payment);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        // POST api/payments
        [HttpPost]
        [Authorize(Roles = "Admin,Guest")]
        public async Task<IActionResult> Create([FromBody] CreatePaymentDto dto)
        {
            try
            {
                var requestingUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var requestingUserRole = User.FindFirstValue(ClaimTypes.Role)!;

                var payment = await _paymentService.CreateAsync(requestingUserId, requestingUserRole, dto);
                return CreatedAtAction(nameof(GetById), new { id = payment.Id }, payment);
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