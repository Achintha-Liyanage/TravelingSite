
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Tours;

public class CreateTourDto
{
    [Required]
    [MaxLength(200)]
    public string? Name { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }

    [Range(0.01, (double)decimal.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Location { get; set; }

    [MaxLength(50)]
    public string? Duration { get; set; }

    [Range(1, 1000)]
    public int MaxGroupSize { get; set; }

    [MaxLength(500)]
    [Url]
    public string? ImageUrl { get; set; }
}
