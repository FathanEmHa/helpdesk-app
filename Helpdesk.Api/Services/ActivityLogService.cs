using Helpdesk.Data;
using Helpdesk.Models;

namespace Helpdesk.Services;

public class ActivityLogService
{
    private readonly AppDbContext _context;
    private readonly CurrentUserService _currentUserService;

    public ActivityLogService(
        AppDbContext context,
        CurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public void Add(
        string action,
        string entityType,
        int entityId,
        string? description = null)
    {
        var activityLog = new ActivityLog
        {
            UserId = _currentUserService.UserId,
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Description = description,
            CreatedAt = DateTime.UtcNow
        };

        _context.ActivityLogs.Add(activityLog);
    }
}