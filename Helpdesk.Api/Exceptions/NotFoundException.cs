using System.Net;

namespace Helpdesk.Exceptions;

public class NotFoundException : AppException
{
    public NotFoundException(string message)
        : base(message, (int)HttpStatusCode.NotFound)
    {
    }
}