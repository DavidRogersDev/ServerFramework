﻿using KesselRun.Web.Api.Messaging.Queries;
using KesselRunFramework.AspNet.Infrastructure;
using KesselRunFramework.AspNet.Infrastructure.Controllers;
using KesselRunFramework.AspNet.Infrastructure.Invariants;
using KesselRunFramework.AspNet.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace KesselRun.Web.Api.Controllers.V1._0
{
    [ApiVersion(Swagger.Versions.v1_0)]
    [Route(AspNet.Mvc.DefaultControllerTemplate)]
    [Produces(MediaTypeNames.Application.Json)]
    public class ColorsController : AppApiMediatrController
    {
        public ColorsController(
            ICurrentUser currentUser,
            ILogger logger,
            IMediator mediator)
            : base(currentUser, logger, mediator)
        {

        }

        [HttpGet]
        [Route(AspNet.Mvc.ActionTemplate)]
        [MapToApiVersion(Swagger.Versions.v1_0)]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetColors()
        {
            var colors = await _mediator.Send(new GetColorsQuery());

            return colors.Match(
                result => OkResponse(colors.LeftOrDefault()), 
                error => UnprocessableEntityResponse(string.Empty, OperationOutcome.ValidationFailOutcome(colors.RightOrDefault().ValidationFailures))
                );
        }
    }
}
