using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class HttpMethodFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var allowedMethods = new[] { "GET" };

        if (!allowedMethods.Contains(context.HttpContext.Request.Method.ToUpper()))
        {
            context.Result = new StatusCodeResult(StatusCodes.Status405MethodNotAllowed);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}