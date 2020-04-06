using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using KesselRun.Business.DataTransferObjects;
using KesselRun.Web.Api.Controllers.V1_0;
using KesselRun.Web.Api.Messaging.Commands;
using KesselRunFramework.AspNet.Infrastructure;
using KesselRunFramework.AspNet.Infrastructure.ActionFilters;
using KesselRunFramework.AspNet.Infrastructure.Controllers;
using KesselRunFramework.AspNet.Infrastructure.Invariants;
using KesselRunFramework.AspNet.Response;
using KesselRunFramework.Core.Infrastructure.Errors;
using KesselRunFramework.Core.Infrastructure.Errors.Api;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KesselRun.Web.Api.Controllers.V1._0
{
    [ApiVersion(Swagger.Versions.v1_0)]
    [Route(AspNet.Mvc.DefaultControllerTemplate)]
    [Produces(MediaTypeNames.Application.Json)]
    public class RegisterUserController : KesselRunApiController
    {
        public RegisterUserController(
            ICurrentUser currentUser,
            ILogger logger,
            IMediator mediator)
            : base(currentUser, logger, mediator)
        {
        }

        [HttpPost]
        [Route(AspNet.Mvc.ActionTemplate)]
        [MapToApiVersion(Swagger.Versions.v1_0)]
        [ApiExplorerSettings(GroupName = Swagger.DocVersions.v1_0)]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<string>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody]RegisterUserPayloadDto dto)
        {
            var result = await _mediator.Send(new RegisterNewUserCommand
            {
                Dto = dto
            });

            return result.IsValidResponse
                ? CreatedAtRoute(routeValues: new
                {
                    controller = "User",
                    action = nameof(UserController.GetUser),
                    id = result.Result.Data,
                    version = HttpContext.GetRequestedApiVersion().ToString()
                }, result.Result)
                : BadRequestResponse(Enumerable.Empty<string>(), operationOutcome: new OperationOutcome
                {
                    OpResult = OpResult.Fail,
                    IsError = false,
                    IsValidationFail = true,
                    Errors = result.Errors
                });
        }
    }
}
