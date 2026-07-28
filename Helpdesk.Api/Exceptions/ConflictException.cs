using System.Net;

namespace Helpdesk.Exceptions;

public class ConflictException : AppException
{
    public ConflictException(string message)
        : base(message, (int)HttpStatusCode.Conflict)
    {
    }
}