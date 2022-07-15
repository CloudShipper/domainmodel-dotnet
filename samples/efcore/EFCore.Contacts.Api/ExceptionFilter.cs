using CloudShipper.DomainModel.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EFCore.Contacts.Api;

public class ExceptionFilter : IActionFilter, IOrderedFilter
{
    public int Order => int.MaxValue - 10;

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (null == context || null == context.Exception)
            return;

        _ = context.Exception switch
        {
            DomainObjectNotFoundException e => context.Result = new NotFoundResult(),
            ValidationException e => context.Result = new BadRequestResult(),
            _ => context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };

        context.ExceptionHandled = true;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
    }
}
