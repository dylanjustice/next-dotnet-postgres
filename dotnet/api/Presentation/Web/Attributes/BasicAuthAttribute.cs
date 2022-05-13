using System;
using Microsoft.AspNetCore.Mvc;
using DylanJustice.Demo.Presentation.Web.Filters.Authorization;

namespace DylanJustice.Demo.Presentation.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class BasicAuthAttribute : TypeFilterAttribute
    {
        public BasicAuthAttribute(string realm = @"GB Realm") : base(typeof(BasicAuthFilter))
        {
            Arguments = new object[] { realm };
        }
    }
}