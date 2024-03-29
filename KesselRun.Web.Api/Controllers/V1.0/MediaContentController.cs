﻿using System.Net.Mime;
using System.Threading.Tasks;
using KesselRun.Business.DataTransferObjects;
using KesselRun.Web.Api.Messaging.Queries;
using KesselRunFramework.AspNet.Infrastructure;
using KesselRunFramework.AspNet.Infrastructure.Controllers;
using KesselRunFramework.AspNet.Infrastructure.Invariants;
using KesselRunFramework.AspNet.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KesselRun.Web.Api.Controllers.V1._0
{
    [ApiVersion(Swagger.Versions.v1_0)]
    [Route(AspNet.Mvc.DefaultControllerTemplate)]
    [Produces(MediaTypeNames.Application.Json)]
    public class MediaContentController : AppApiMediatrController
    {
        public MediaContentController(ICurrentUser currentUser, ILogger logger, IMediator mediator) 
            : base(currentUser, logger, mediator)
        {
        }

        [HttpGet]
        [Route(AspNet.Mvc.ActionTemplate)]
        [MapToApiVersion(Swagger.Versions.v1_0)]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTvShow([FromQuery]TvShowPayloadDto tvShowPayload)
        {
            var tvShow = await _mediator.Send(new GetTvShowQuery { Season = tvShowPayload.Season, Title = tvShowPayload.Title });

            return tvShow.Match(
                t => OkResponse(tvShow.LeftOrDefault()),
                p => InternalServerErrorResponse(tvShow.RightOrDefault(), OperationOutcome.UnSuccessfulOutcome)
            );

        }
    }
}
