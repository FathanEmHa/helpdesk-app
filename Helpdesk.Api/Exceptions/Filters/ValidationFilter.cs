using Helpdesk.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Helpdesk.Filters;

public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid)
            return;

        var errors = context.ModelState
            .Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        throw new ValidationException(errors);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}