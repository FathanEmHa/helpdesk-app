using System.Net;

namespace Helpdesk.Exceptions;

public class ValidationException : AppException
{
    public ValidationException(string message)
        : base(message, (int)HttpStatusCode.BadRequest)
    {
    }
}