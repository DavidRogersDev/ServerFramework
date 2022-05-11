using KesselRunFramework.AspNet.Infrastructure.Bootstrapping;
using KesselRunFramework.AspNet.Response;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace KesselRunFramework.AspNet.Infrastructure.Extensions
{
    public static class ModelStateExtensions
    {
        public static UnprocessableEntityPayload ToUnprocessablePayload(this ModelStateDictionary source)
        {
            var errors = Common.GetErrorsFromModelState(source);

            return new UnprocessableEntityPayload
            {
                Errors = errors
            };
        }
    }
}
