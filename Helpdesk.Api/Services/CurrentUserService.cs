using System.Security.Claims;
using Helpdesk.Data;
using Helpdesk.Exceptions;
using Helpdesk.Models;

namespace Helpdesk.Services;

public class CurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppDbContext _context;

    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor,
        AppDbContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }

    public async Task<User> GetAsync()
    {
        var principal = _httpContextAccessor.HttpContext?.User;

        if (principal == null)
            throw new UnauthorizedAccessException();

        var idClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

        if (idClaim == null)
            throw new UnauthorizedAccessException();

        var userId = int.Parse(idClaim.Value);

        var user = await _context.Users.FindAsync(userId);

        if (user == null)
            throw new UnauthorizedAccessException();

        return user;
    }
}