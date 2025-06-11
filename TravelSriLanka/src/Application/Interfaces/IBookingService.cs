
using Application.DTOs.Bookings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IBookingService
{
    Task<BookingDto?> CreateBookingAsync(CreateBookingDto createBookingDto, int userId);
    Task<IEnumerable<BookingDto>> GetUserBookingsAsync(int userId);
    Task<BookingDto?> GetBookingByIdAsync(int bookingId, int userId); // User can only get their own booking
    Task<IEnumerable<BookingDto>> GetAllBookingsAsync(); // For Admin
    Task<bool> UpdateBookingStatusAsync(int bookingId, UpdateBookingStatusDto updateBookingStatusDto); // For Admin
    // Add method to get bookings for a specific tour (for admin or tour operator)
    Task<IEnumerable<BookingDto>> GetTourBookingsAsync(int tourId);
}
