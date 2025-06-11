
using Application.DTOs.Bookings;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
// using Microsoft.EntityFrameworkCore; // Required for Include - Removed as Application layer should not directly use EFCore features
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services;

public class BookingService : IBookingService
{
    private readonly IGenericRepository<Booking> _bookingRepository;
    private readonly IGenericRepository<Tour> _tourRepository;
    private readonly IGenericRepository<User> _userRepository; // To get user details if needed

    public BookingService(
        IGenericRepository<Booking> bookingRepository,
        IGenericRepository<Tour> tourRepository,
        IGenericRepository<User> userRepository)
    {
        _bookingRepository = bookingRepository;
        _tourRepository = tourRepository;
        _userRepository = userRepository;
    }

    public async Task<BookingDto?> CreateBookingAsync(CreateBookingDto createBookingDto, int userId)
    {
        var tour = await _tourRepository.GetByIdAsync(createBookingDto.TourId);
        if (tour == null)
        {
            // Tour not found
            return null;
        }

        // Rudimentary availability check (can be expanded)
        if (createBookingDto.NumberOfPeople > tour.MaxGroupSize)
        {
            // Not enough capacity
            // Consider throwing a specific exception or returning a result object
            return null;
        }

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            // User not found - should not happen if userId is from authenticated context
            return null;
        }

        var booking = new Booking
        {
            TourId = createBookingDto.TourId,
            UserId = userId,
            NumberOfPeople = createBookingDto.NumberOfPeople,
            TotalPrice = tour.Price * createBookingDto.NumberOfPeople, // Simple price calculation
            BookingDate = DateTime.UtcNow,
            Status = BookingStatus.Pending
        };

        await _bookingRepository.AddAsync(booking);
        await _bookingRepository.SaveChangesAsync();

        // Manual Map to DTO
        return new BookingDto
        {
            Id = booking.Id,
            TourId = booking.TourId,
            TourName = tour.Name,
            UserId = booking.UserId,
            UserName = user.Username, // Assuming User has Username
            BookingDate = booking.BookingDate,
            NumberOfPeople = booking.NumberOfPeople,
            TotalPrice = booking.TotalPrice,
            Status = booking.Status
        };
    }

    public async Task<IEnumerable<BookingDto>> GetUserBookingsAsync(int userId)
    {
        // Need to query with Include for related data. GenericRepository might need extension or use DbContext directly for complex queries.
        // For now, let's assume GenericRepository can be extended or we do a simpler fetch.
        // This highlights a limitation of a very generic repository for complex reads.
        // Let's adjust to use what we have for now and improve if needed.

        var bookings = (await _bookingRepository.GetAllAsync()) // This is inefficient
                        .Where(b => b.UserId == userId).ToList();

        var bookingDtos = new List<BookingDto>();
        foreach(var booking in bookings)
        {
            var tour = await _tourRepository.GetByIdAsync(booking.TourId);
            var user = await _userRepository.GetByIdAsync(booking.UserId);
            bookingDtos.Add(new BookingDto {
                Id = booking.Id,
                TourId = booking.TourId,
                TourName = tour?.Name,
                UserId = booking.UserId,
                UserName = user?.Username,
                BookingDate = booking.BookingDate,
                NumberOfPeople = booking.NumberOfPeople,
                TotalPrice = booking.TotalPrice,
                Status = booking.Status
            });
        }
        return bookingDtos;
    }

    public async Task<BookingDto?> GetBookingByIdAsync(int bookingId, int userId)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);
        if (booking == null || booking.UserId != userId) return null; // Security check

        var tour = await _tourRepository.GetByIdAsync(booking.TourId);
        var user = await _userRepository.GetByIdAsync(booking.UserId);

        return new BookingDto {
            Id = booking.Id,
            TourId = booking.TourId,
            TourName = tour?.Name,
            UserId = booking.UserId,
            UserName = user?.Username,
            BookingDate = booking.BookingDate,
            NumberOfPeople = booking.NumberOfPeople,
            TotalPrice = booking.TotalPrice,
            Status = booking.Status
        };
    }

    public async Task<IEnumerable<BookingDto>> GetAllBookingsAsync() // For Admin
    {
        var bookings = await _bookingRepository.GetAllAsync(); // Inefficient
        var bookingDtos = new List<BookingDto>();
        foreach(var booking in bookings)
        {
            var tour = await _tourRepository.GetByIdAsync(booking.TourId);
            var user = await _userRepository.GetByIdAsync(booking.UserId); // N+1 problem here
            bookingDtos.Add(new BookingDto {
                Id = booking.Id,
                TourId = booking.TourId,
                TourName = tour?.Name,
                UserId = booking.UserId,
                UserName = user?.Username,
                BookingDate = booking.BookingDate,
                NumberOfPeople = booking.NumberOfPeople,
                TotalPrice = booking.TotalPrice,
                Status = booking.Status
            });
        }
        return bookingDtos;
    }

    public async Task<IEnumerable<BookingDto>> GetTourBookingsAsync(int tourId)
    {
        var bookings = (await _bookingRepository.GetAllAsync()).Where(b => b.TourId == tourId).ToList();
        var bookingDtos = new List<BookingDto>();
        foreach(var booking in bookings)
        {
            var tour = await _tourRepository.GetByIdAsync(booking.TourId); // Redundant if we know tourId
            var user = await _userRepository.GetByIdAsync(booking.UserId);
            bookingDtos.Add(new BookingDto {
                Id = booking.Id,
                TourId = booking.TourId,
                TourName = tour?.Name, // Could pass tour name directly
                UserId = booking.UserId,
                UserName = user?.Username,
                BookingDate = booking.BookingDate,
                NumberOfPeople = booking.NumberOfPeople,
                TotalPrice = booking.TotalPrice,
                Status = booking.Status
            });
        }
        return bookingDtos;
    }

    public async Task<bool> UpdateBookingStatusAsync(int bookingId, UpdateBookingStatusDto updateBookingStatusDto)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);
        if (booking == null) return false;

        booking.Status = updateBookingStatusDto.Status;
        _bookingRepository.Update(booking);
        await _bookingRepository.SaveChangesAsync();
        return true;
    }
}
