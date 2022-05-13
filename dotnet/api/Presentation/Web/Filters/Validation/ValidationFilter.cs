using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using DylanJustice.Demo.Presentation.Web.Extensions.Validation;

namespace DylanJustice.Demo.Presentation.Web.Filters.Validation
{
    public class ValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }

            // Return an IResult containing all validation errors
            context.Result = new BadRequestObjectResult(context.ModelState.ToResult<object>());
        }
    }
}
