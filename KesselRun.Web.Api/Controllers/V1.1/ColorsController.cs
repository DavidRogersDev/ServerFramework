using KesselRunFramework.AspNet.Infrastructure;
using KesselRunFramework.AspNet.Infrastructure.Controllers;
using KesselRunFramework.AspNet.Infrastructure.Invariants;
using KesselRunFramework.AspNet.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using KesselRun.Business.DataTransferObjects;
using KesselRunFramework.Core.Infrastructure.Validation;
using System.Threading;
using KesselRunFramework.Core.Cqrs.Queries;
using KesselRun.Web.Api.Messaging.Queries;
using KesselRunFramework.Core.Infrastructure.Messaging;
using KesselRunFramework.AspNet.Infrastructure.Extensions;

namespace KesselRun.Web.Api.Controllers.V1._1
{
    [ApiVersion(Swagger.Versions.v1_1)]
    [Route(AspNet.Mvc.DefaultControllerTemplate)]
    [Produces(MediaTypeNames.Application.Json)]
    public class ColorsController : AppApiController
    {
        private readonly IQueryHandler<GetColorsQuery, Either<IEnumerable<ColorPayloadDto>, ValidateableResponse>> _queryHandler;

        public ColorsController(
            ICurrentUser currentUser,
            ILogger logger,
            IQueryHandler<GetColorsQuery, Either<IEnumerable<ColorPayloadDto>, ValidateableResponse>> queryHandler
            )
            : base(currentUser)
        {
            _queryHandler = queryHandler;
        }

        [HttpGet]
        [Route(AspNet.Mvc.ActionTemplate)]
        [MapToApiVersion(Swagger.Versions.v1_1)]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetColors()
        {
            var colors = await _queryHandler.HandleAsync(new GetColorsQuery(), CancellationToken.None);

            return colors.Match(
                    result => OkResponse(colors.LeftOrDefault().Concat(new[] { new ColorPayloadDto { Color = "Yay for 1.1" } })),
                    error => colors.RightOrDefault().ToUnprocessableRequestResult()
                );
        }
    }
}
