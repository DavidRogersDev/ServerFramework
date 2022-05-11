using KesselRunFramework.Core.Infrastructure.Messaging;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace KesselRunFramework.AspNet.Infrastructure.Extensions
{
    public static class ValidateableResponseExtensions
    {
        public static void AddToModelState(this ValidateableResponse source, ModelStateDictionary modelState)
        {
            foreach (var error in source.Errors)
            {
                modelState.AddModelError("ServiceFail", error);
            }
        }
    }
}
