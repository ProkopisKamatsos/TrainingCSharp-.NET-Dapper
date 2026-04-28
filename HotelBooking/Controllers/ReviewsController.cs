using System.Security.Claims;
using HotelBooking.DTOs.Review;
using HotelBooking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // GET api/reviews/hotel/1
        [HttpGet("hotel/{hotelId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByHotelId(int hotelId)
        {
            var reviews = await _reviewService.GetByHotelIdAsync(hotelId);
            return Ok(reviews);
        }

        // POST api/reviews
        [HttpPost]
        [Authorize(Roles = "Admin,Guest")]
        public async Task<IActionResult> Create([FromBody] CreateReviewDto dto)
        {
            try
            {
                var requestingUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var requestingUserRole = User.FindFirstValue(ClaimTypes.Role)!;

                var review = await _reviewService.CreateAsync(requestingUserId, requestingUserRole, dto);
                return CreatedAtAction(nameof(GetByHotelId), new { hotelId = review.HotelId }, review);
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