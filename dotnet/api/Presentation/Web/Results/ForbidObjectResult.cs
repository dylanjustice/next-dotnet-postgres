using Microsoft.AspNetCore.Mvc;

namespace DylanJustice.Demo.Presentation.Web.Results
{
    public class ForbidObjectResult : ObjectResult
    {
        public ForbidObjectResult(object value) : base(value)
        {
            StatusCode = 403;
        }
    }
}