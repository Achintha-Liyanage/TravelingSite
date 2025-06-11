
using Application.DTOs.Auth;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
    {
        var user = await _authService.RegisterAsync(userForRegisterDto);
        if (user == null)
        {
            return BadRequest("Username already exists.");
        }
        // Optionally return the created user or a success message
        return Ok(new { user.Id, user.Username, user.Role });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
    {
        var token = await _authService.LoginAsync(userForLoginDto);
        if (token == null)
        {
            return Unauthorized("Invalid credentials.");
        }
        return Ok(new TokenDto { Token = token });
    }
}
