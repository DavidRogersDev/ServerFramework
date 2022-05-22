using System.Collections.Generic;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using KesselRun.Business.DataTransferObjects;
using KesselRun.Web.Api.Controllers.V1_0;
using KesselRun.Web.Api.Messaging.Commands;
using KesselRunFramework.AspNet.Infrastructure;
using KesselRunFramework.AspNet.Infrastructure.Controllers;
using KesselRunFramework.AspNet.Infrastructure.Extensions;
using KesselRunFramework.AspNet.Infrastructure.Invariants;
using KesselRunFramework.AspNet.Response;
using KesselRunFramework.Core.Cqrs.Commands;
using KesselRunFramework.Core.Infrastructure.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KesselRun.Web.Api.Controllers.V1._0
{
    [ApiVersion(Swagger.Versions.v1_0)]
    [Route(AspNet.Mvc.DefaultControllerTemplate)]
    [Produces(MediaTypeNames.Application.Json)]
    public class RegisterUserController : AppApiController
    {
        private readonly ICommandHandler<RegisterNewUserCommand, ValidateableResponse<ApiResponse<int>>> handler;

        public RegisterUserController(
            ICurrentUser currentUser,
            ILogger logger,
            ICommandHandler<RegisterNewUserCommand, ValidateableResponse<ApiResponse<int>>> handler)
            : base(currentUser)
        {
            this.handler = handler;
        }

        [HttpPost]
        [Route(AspNet.Mvc.ActionTemplate)]
        [MapToApiVersion(Swagger.Versions.v1_0)]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<string>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromForm]RegisterUserPayloadDto dto)
        {
            var result = await handler.ExecuteAsync(new RegisterNewUserCommand
            {
                Dto = dto
            }, CancellationToken.None);

            return result.IsValidResponse
                ? CreatedAtRoute(routeValues: new
                {
                    controller = "User",
                    action = nameof(UserController.GetUser),
                    id = result.Result.Data,
                    version = HttpContext.GetRequestedApiVersion().ToString()
                }, result.Result)
                : result.ToUnprocessableRequestResult();
        }
    }
}
