using Helpdesk.Exceptions;
using Helpdesk.Models;

namespace Helpdesk.Helpers;

public static class AuthorizationHelper
{
    public static void EnsureAdmin(User currentUser)
    {
        if (currentUser.Role != Role.Admin)
            throw new ForbiddenException("Access denied.");
    }

    public static void EnsureOwnerOrAdmin(int ownerId, User currentUser)
    {
        if (currentUser.Role == Role.Admin)
            return;

        if (currentUser.Id != ownerId)
            throw new ForbiddenException("Access denied.");
    }
}