
using Domain.Entities; // For BookingStatus enum
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Bookings;

public class UpdateBookingStatusDto
{
    [Required]
    public BookingStatus Status { get; set; }
}
