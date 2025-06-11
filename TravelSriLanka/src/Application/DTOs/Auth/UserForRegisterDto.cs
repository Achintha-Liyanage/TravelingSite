
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth;

public class UserForRegisterDto
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    public string Role { get; set; } = "User"; // Default role
}
