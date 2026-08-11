using Helpdesk.Services;
using Helpdesk.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Helpdesk.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<IActionResult> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        return Ok(await _authService.Login(
            request,
            cancellationToken));
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return Ok(new
        {
            message = "Logout successful."
        });
    }
}