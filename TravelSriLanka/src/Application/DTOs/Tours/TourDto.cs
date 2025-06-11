
namespace Application.DTOs.Tours;

public class TourDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? Location { get; set; }
    public string? Duration { get; set; }
    public int MaxGroupSize { get; set; }
    public string? ImageUrl { get; set; }
}
