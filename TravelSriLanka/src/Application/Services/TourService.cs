
using Application.DTOs.Tours;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services;

public class TourService : ITourService
{
    private readonly IGenericRepository<Tour> _tourRepository;
    // In a real app, inject AutoMapper's IMapper here

    public TourService(IGenericRepository<Tour> tourRepository)
    {
        _tourRepository = tourRepository;
    }

    public async Task<IEnumerable<TourDto>> GetAllToursAsync()
    {
        var tours = await _tourRepository.GetAllAsync();
        // Manual mapping (replace with AutoMapper in a real app)
        return tours.Select(tour => new TourDto
        {
            Id = tour.Id,
            Name = tour.Name,
            Description = tour.Description,
            Price = tour.Price,
            Location = tour.Location,
            Duration = tour.Duration,
            MaxGroupSize = tour.MaxGroupSize,
            ImageUrl = tour.ImageUrl
        }).ToList();
    }

    public async Task<TourDto?> GetTourByIdAsync(int id)
    {
        var tour = await _tourRepository.GetByIdAsync(id);
        if (tour == null) return null;

        // Manual mapping
        return new TourDto
        {
            Id = tour.Id,
            Name = tour.Name,
            Description = tour.Description,
            Price = tour.Price,
            Location = tour.Location,
            Duration = tour.Duration,
            MaxGroupSize = tour.MaxGroupSize,
            ImageUrl = tour.ImageUrl
        };
    }

    public async Task<TourDto?> CreateTourAsync(CreateTourDto createTourDto)
    {
        // Manual mapping
        var tour = new Tour
        {
            Name = createTourDto.Name,
            Description = createTourDto.Description,
            Price = createTourDto.Price,
            Location = createTourDto.Location,
            Duration = createTourDto.Duration,
            MaxGroupSize = createTourDto.MaxGroupSize,
            ImageUrl = createTourDto.ImageUrl
        };

        await _tourRepository.AddAsync(tour);
        await _tourRepository.SaveChangesAsync();

        // Manual mapping to return DTO
        return new TourDto {
            Id = tour.Id, // Id is generated after save
            Name = tour.Name,
            Description = tour.Description,
            Price = tour.Price,
            Location = tour.Location,
            Duration = tour.Duration,
            MaxGroupSize = tour.MaxGroupSize,
            ImageUrl = tour.ImageUrl
        };
    }

    public async Task<bool> UpdateTourAsync(int id, UpdateTourDto updateTourDto)
    {
        var tour = await _tourRepository.GetByIdAsync(id);
        if (tour == null) return false;

        // Manual mapping
        tour.Name = updateTourDto.Name;
        tour.Description = updateTourDto.Description;
        tour.Price = updateTourDto.Price;
        tour.Location = updateTourDto.Location;
        tour.Duration = updateTourDto.Duration;
        tour.MaxGroupSize = updateTourDto.MaxGroupSize;
        tour.ImageUrl = updateTourDto.ImageUrl;

        _tourRepository.Update(tour);
        await _tourRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteTourAsync(int id)
    {
        var tour = await _tourRepository.GetByIdAsync(id);
        if (tour == null) return false;

        _tourRepository.Delete(tour);
        await _tourRepository.SaveChangesAsync();
        return true;
    }
}
