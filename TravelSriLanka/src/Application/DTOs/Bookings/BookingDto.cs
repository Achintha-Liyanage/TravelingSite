
using Domain.Entities; // For BookingStatus enum
using System;

namespace Application.DTOs.Bookings;

public class BookingDto
{
    public int Id { get; set; }
    public int TourId { get; set; }
    public string? TourName { get; set; } // For display
    public int UserId { get; set; }
    public string? UserName { get; set; } // For display
    public DateTime BookingDate { get; set; }
    public int NumberOfPeople { get; set; }
    public decimal TotalPrice { get; set; }
    public BookingStatus Status { get; set; }
}
