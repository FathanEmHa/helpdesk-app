using System.Net;

namespace Helpdesk.Exceptions;

public class ValidationException : AppException
{
    public List<string> Errors { get; }

    public ValidationException(List<string> errors)
        : base("Validation failed.", (int)HttpStatusCode.BadRequest)
    {
        Errors = errors;
    }

    public ValidationException(string error)
        : this(new List<string> { error })
    {
    }
}