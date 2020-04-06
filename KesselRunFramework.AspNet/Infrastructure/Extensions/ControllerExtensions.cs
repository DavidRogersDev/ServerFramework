using KesselRunFramework.AspNet.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KesselRunFramework.AspNet.Infrastructure.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult Forbidden<T>(this ControllerBase source, ApiResponse<T> apiResponse)
        {
            return source.StatusCode(StatusCodes.Status403Forbidden, apiResponse);
        }

        public static IActionResult ServerError(this ControllerBase source, string errors)
        {
            return source.StatusCode(StatusCodes.Status500InternalServerError, errors);
        }

        public static IActionResult ServerError<T>(this ControllerBase source, ApiResponse<T> apiResponse)
        {
            return source.StatusCode(StatusCodes.Status500InternalServerError, apiResponse);
        }
    }
}
