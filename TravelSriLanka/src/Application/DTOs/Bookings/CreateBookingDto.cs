
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Bookings;

public class CreateBookingDto
{
    [Required]
    public int TourId { get; set; }

    [Required]
    [Range(1, 100)]
    public int NumberOfPeople { get; set; }

    // UserId will be taken from the authenticated user context
}
