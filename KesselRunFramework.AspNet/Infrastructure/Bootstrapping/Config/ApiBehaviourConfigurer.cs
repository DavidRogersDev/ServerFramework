using Microsoft.AspNetCore.Mvc;

namespace KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Config
{
    public class ApiBehaviourConfigurer
    {
        public static void ConfigureApiBehaviour(ApiBehaviorOptions apiBehaviorOptions)
        {
            apiBehaviorOptions.InvalidModelStateResponseFactory = context =>
            {
                var payload = Common.ProcessInvalidModelState(context.ModelState);
                
                return new BadRequestObjectResult(payload);
            };
        }
    }
}
