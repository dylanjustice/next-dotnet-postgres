using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using DylanJustice.Demo.Presentation.Web.Constants;
using AndcultureCode.CSharp.Core.Utilities.Localization;

namespace DylanJustice.Demo.Presentation.Web.Middleware.Localization
{
    public class CultureRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.ContainsKey(ApiSettings.ROUTING_CULTURE_CONSTRAINT))
            {
                return false;
            }

            var cultureCode = values[ApiSettings.ROUTING_CULTURE_CONSTRAINT].ToString();

            return LocalizationUtils.CultureExists(cultureCode);
        }
    }
}