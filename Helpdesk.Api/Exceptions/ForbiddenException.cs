using System.Net;

namespace Helpdesk.Exceptions;

public class ForbiddenException : AppException
{
    public ForbiddenException(string message)
        : base(message, (int)HttpStatusCode.Forbidden)
    {
    }
}