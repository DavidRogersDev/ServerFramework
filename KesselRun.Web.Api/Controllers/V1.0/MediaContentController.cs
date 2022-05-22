using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using KesselRun.Business.DataTransferObjects;
using KesselRun.Business.DataTransferObjects.Media;
using KesselRun.Web.Api.Messaging.Queries;
using KesselRunFramework.AspNet.Infrastructure;
using KesselRunFramework.AspNet.Infrastructure.Controllers;
using KesselRunFramework.AspNet.Infrastructure.Invariants;
using KesselRunFramework.AspNet.Response;
using KesselRunFramework.Core.Cqrs.Queries;
using KesselRunFramework.Core.Infrastructure.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KesselRun.Web.Api.Controllers.V1._0
{
    [ApiVersion(Swagger.Versions.v1_0)]
    [Route(AspNet.Mvc.DefaultControllerTemplate)]
    [Produces(MediaTypeNames.Application.Json)]
    public class MediaContentController : AppApiController
    {
        private readonly IQueryHandler<GetTvShowQuery, Either<TvShow, ProblemDetails>> _queryHandler;

        public MediaContentController(ICurrentUser currentUser, IQueryHandler<GetTvShowQuery, Either<TvShow, ProblemDetails>> queryHandler) 
            : base(currentUser)
        {
            _queryHandler = queryHandler;
        }

        [HttpGet]
        [Route(AspNet.Mvc.ActionTemplate)]
        [MapToApiVersion(Swagger.Versions.v1_0)]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTvShow([FromQuery]TvShowPayloadDto tvShowPayload)
        {
            var tvShow = await _queryHandler.HandleAsync(new GetTvShowQuery { Season = tvShowPayload.Season, Title = tvShowPayload.Title }, CancellationToken.None);

            return tvShow.Match(
                t => OkResponse(tvShow.LeftOrDefault()),
                p => InternalServerErrorResponse(tvShow.RightOrDefault(), OperationOutcome.UnSuccessfulOutcome)
            );

        }
    }
}
