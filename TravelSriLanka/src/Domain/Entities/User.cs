using System.Collections.Generic;

namespace Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
    public string Role { get; set; } = string.Empty; // e.g., Admin, User
    // Add other user properties like Email, Name etc. later
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
