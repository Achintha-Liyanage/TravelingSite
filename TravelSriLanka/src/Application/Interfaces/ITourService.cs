
using Application.DTOs.Tours;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface ITourService
{
    Task<IEnumerable<TourDto>> GetAllToursAsync();
    Task<TourDto?> GetTourByIdAsync(int id); // Nullable if not found
    Task<TourDto?> CreateTourAsync(CreateTourDto createTourDto);
    Task<bool> UpdateTourAsync(int id, UpdateTourDto updateTourDto);
    Task<bool> DeleteTourAsync(int id);
}
