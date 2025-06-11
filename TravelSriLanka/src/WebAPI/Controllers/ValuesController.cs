
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    [HttpGet("public")]
    public IActionResult GetPublic()
    {
        return Ok("This is a public endpoint.");
    }

    [HttpGet("secured")]
    [Authorize] // Requires any authenticated user
    public IActionResult GetSecured()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var userName = User.Identity?.Name;
        return Ok(new { Message = "This is a secured endpoint.", UserId = userId, UserName = userName });
    }

    [HttpGet("admin")]
    [Authorize(Roles = "Admin")] // Requires user with Admin role
    public IActionResult GetAdmin()
    {
        return Ok("This is an admin-only endpoint.");
    }
}
