using Helpdesk.Data;
using Helpdesk.Models;

namespace Helpdesk.Services;

public class CurrentUserService
{
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly AppDbContext _context;

    public CurrentUserService(
        ICurrentUserAccessor currentUserAccessor,
        AppDbContext context)
    {
        _currentUserAccessor = currentUserAccessor;
        _context = context;
    }

    public int? UserId => _currentUserAccessor.UserId;

    public async Task<User> GetAsync()
    {
        if (UserId == null)
            throw new UnauthorizedAccessException();

        var user = await _context.Users.FindAsync(UserId.Value);

        if (user == null)
            throw new UnauthorizedAccessException();

        return user;
    }
}