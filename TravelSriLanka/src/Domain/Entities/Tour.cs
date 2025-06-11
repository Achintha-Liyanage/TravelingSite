
using System.Collections.Generic; // Added this
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Tour
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string? Name { get; set; } // Made nullable to match previous fix for CS8618

    [MaxLength(2000)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(18,2)")] // Escaped parentheses
    public decimal Price { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Location { get; set; }

    [MaxLength(50)]
    public string? Duration { get; set; } // e.g., "7 Days", "3 Hours"

    public int MaxGroupSize { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    // Consider adding a collection of Bookings or Reviews if needed for navigation
    // public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    // public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>(); // Added this
}
