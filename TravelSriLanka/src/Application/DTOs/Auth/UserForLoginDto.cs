
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth;

public class UserForLoginDto
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
