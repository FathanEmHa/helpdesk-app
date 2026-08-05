namespace Helpdesk.Services;

public interface ICurrentUserAccessor
{
    int? UserId { get; }
}