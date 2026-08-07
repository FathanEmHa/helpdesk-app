using Helpdesk.Data;
using Helpdesk.Models;

namespace Helpdesk.Services.Base;

public abstract class BaseService
{
    protected readonly AppDbContext Context;
    protected readonly CurrentUserService CurrentUserService;

    protected int? CurrentUserId =>
        CurrentUserService.UserId;

    protected BaseService(
        AppDbContext context,
        CurrentUserService currentUserService)
    {
        Context = context;
        CurrentUserService = currentUserService;
    }

    protected Task<User> GetCurrentUser(
        CancellationToken cancellationToken = default)
    {
        return CurrentUserService.GetAsync(
            cancellationToken);
    }
}