
using Application.DTOs.Bookings;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] // All booking operations require authentication
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            throw new InvalidOperationException("User ID not found in token or invalid.");
        }
        return userId;
    }

    [HttpPost]
    public async Task<ActionResult<BookingDto>> CreateBooking(CreateBookingDto createBookingDto)
    {
        var userId = GetUserId();
        var booking = await _bookingService.CreateBookingAsync(createBookingDto, userId);
        if (booking == null)
        {
            // Could be Tour not found, or MaxGroupSize exceeded, etc.
            return BadRequest("Could not create booking. Tour may not exist or capacity exceeded.");
        }
        // Return a 201 Created with the location of the new resource (individual booking endpoint)
        return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, booking);
    }

    [HttpGet("my-bookings")]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetMyBookings()
    {
        var userId = GetUserId();
        var bookings = await _bookingService.GetUserBookingsAsync(userId);
        return Ok(bookings);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookingDto>> GetBookingById(int id)
    {
        var userId = GetUserId();
        var booking = await _bookingService.GetBookingByIdAsync(id, userId);
        if (booking == null)
        {
            return NotFound("Booking not found or you do not have permission to view it.");
        }
        return Ok(booking);
    }

    // Admin Endpoints
    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetAllBookings()
    {
        var bookings = await _bookingService.GetAllBookingsAsync();
        return Ok(bookings);
    }

    [HttpGet("tour/{tourId}")]
    [Authorize(Roles = "Admin")] // Or a specific role like TourManager
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookingsForTour(int tourId)
    {
        var bookings = await _bookingService.GetTourBookingsAsync(tourId);
        return Ok(bookings);
    }

    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateBookingStatus(int id, UpdateBookingStatusDto updateDto)
    {
        var success = await _bookingService.UpdateBookingStatusAsync(id, updateDto);
        if (!success)
        {
            return NotFound("Booking not found.");
        }
        return NoContent();
    }
}
