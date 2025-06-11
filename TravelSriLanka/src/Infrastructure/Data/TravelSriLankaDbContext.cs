
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class TravelSriLankaDbContext : DbContext
{
    public TravelSriLankaDbContext(DbContextOptions<TravelSriLankaDbContext> options) : base(options)
    {
    }

    public DbSet<Tour> Tours { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<User> Users { get; set; }

    // OnModelCreating can be added later as entities are defined
}
