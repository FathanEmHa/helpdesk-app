using Helpdesk.Services;
using Helpdesk.Dtos;
using Microsoft.AspNetCore.Mvc;

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
}