using Helpdesk.Services;
using Helpdesk.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Helpdesk.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    // [HttpPost("register")]
    // public async Task<IActionResult> Register(RegisterRequest request)
    // {
    //     var user = await _authService.Register(request);

    //     return Created("", new
    //     {
    //         user.Id,
    //         user.Name,
    //         user.Email,
    //         user.PhoneNumber
    //     });
    // }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _authService.Login(request);

        return Ok(result);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userId, out var id))
            return Unauthorized();

        var result = await _authService.Me(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
}