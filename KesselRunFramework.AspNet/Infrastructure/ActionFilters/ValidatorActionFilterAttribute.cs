using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using KesselRunFramework.AspNet.Infrastructure.Bootstrapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KesselRunFramework.AspNet.Infrastructure.ActionFilters
{
    /// <summary>
    /// This attribute is for AJAX posts. No need to include the ModelState.IsValid check 
    /// in the action method of the controller, as that check is performed here.
    /// </summary>
    public partial class ValidatorActionFilterAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            if (context.ModelState.IsValid)
            {
                await next();
            }
            else
            {

                var payload = Common.ProcessInvalidModelState(context.ModelState);

                var serializedModelState = JsonSerializer.Serialize(payload, Common.JsonSerializerOptions);

                var result = new ContentResult
                {
                    Content = serializedModelState,
                    ContentType = MediaTypeNames.Application.Json
                };

                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Result = result;
            }
        }
    }
}
