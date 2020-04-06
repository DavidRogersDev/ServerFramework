using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace KesselRunFramework.AspNet.Infrastructure.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SerilogMvcLoggingAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var diagnosticContext = context.HttpContext.RequestServices.GetService<IDiagnosticContext>();

            diagnosticContext.Set("RouteData", context.ActionDescriptor.RouteValues);
            diagnosticContext.Set("ActionName", context.ActionDescriptor.DisplayName);
            diagnosticContext.Set("ActionId", context.ActionDescriptor.Id);
            diagnosticContext.Set("ValidationState", context.ModelState.IsValid);
        }
    }
}
