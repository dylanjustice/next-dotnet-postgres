using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DylanJustice.Demo.Presentation.Web.Filters
{
    public class NotAllowedFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.Result = new UnauthorizedResult();
        }
    }
}