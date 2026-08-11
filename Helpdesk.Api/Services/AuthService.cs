using BCrypt.Net;
using Helpdesk.Data;
using Helpdesk.Dtos.Auth;
using Helpdesk.Models;
using Helpdesk.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.Services;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly JwtService _jwtService;

    public AuthService(
        AppDbContext context,
        JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<AuthResponse> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                u =>
                    u.Email == request.Email &&
                    u.Status == UserStatus.Active,
                cancellationToken);

        if (user == null)
            throw new UnauthorizedException(
                "Invalid email or password.");

        var validPassword = BCrypt.Net.BCrypt.Verify(
            request.Password,
            user.PasswordHash);

        if (!validPassword)
            throw new UnauthorizedException(
                "Invalid email or password.");

        var token = _jwtService.GenerateToken(user);

        return new AuthResponse
        {
            Token = token,
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }
}