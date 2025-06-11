
using Application.DTOs.Tours;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ToursController : ControllerBase
{
    private readonly ITourService _tourService;

    public ToursController(ITourService tourService)
    {
        _tourService = tourService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TourDto>>> GetAllTours()
    {
        var tours = await _tourService.GetAllToursAsync();
        return Ok(tours);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TourDto>> GetTourById(int id)
    {
        var tour = await _tourService.GetTourByIdAsync(id);
        if (tour == null)
        {
            return NotFound();
        }
        return Ok(tour);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")] // Only Admins can create tours
    public async Task<ActionResult<TourDto>> CreateTour(CreateTourDto createTourDto)
    {
        var createdTour = await _tourService.CreateTourAsync(createTourDto);
        if (createdTour == null)
        {
            // This case might occur if there's a validation or creation issue not throwing an exception
            return BadRequest("Could not create tour.");
        }
        return CreatedAtAction(nameof(GetTourById), new { id = createdTour.Id }, createdTour);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")] // Only Admins can update tours
    public async Task<IActionResult> UpdateTour(int id, UpdateTourDto updateTourDto)
    {
        var success = await _tourService.UpdateTourAsync(id, updateTourDto);
        if (!success)
        {
            return NotFound(); // Or BadRequest if validation fails but tour exists
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // Only Admins can delete tours
    public async Task<IActionResult> DeleteTour(int id)
    {
        var success = await _tourService.DeleteTourAsync(id);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }
}
