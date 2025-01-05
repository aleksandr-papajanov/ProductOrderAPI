using Microsoft.AspNetCore.Mvc.Filters;
using ProductOrderApi.Helpers;
using ProductOrderApi.Helpers.Exceptions;

namespace ProductOrderApi.Middleware
{
    internal class ValidateModelStateFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                throw new ValidationApiException(context.ModelState);
            }
        }
    }
}
