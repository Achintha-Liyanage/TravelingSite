
using Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public enum BookingStatus
{
    Pending,
    Confirmed,
    Cancelled,
    Completed
}

public class Booking
{
    public int Id { get; set; }

    [Required]
    public int TourId { get; set; }
    public virtual Tour? Tour { get; set; } // Navigation property

    [Required]
    public int UserId { get; set; } // Assuming User.Id is int
    public virtual User? User { get; set; } // Navigation property

    [Required]
    public DateTime BookingDate { get; set; } = DateTime.UtcNow;

    [Range(1, 100)] // Max 100 people per booking
    public int NumberOfPeople { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; }

    public BookingStatus Status { get; set; } = BookingStatus.Pending;

    // Add other properties like PaymentIntentId if integrating with Stripe, etc.
}
