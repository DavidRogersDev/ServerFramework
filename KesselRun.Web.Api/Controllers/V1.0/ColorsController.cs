using FluentValidation.Results;
using KesselRun.Business.DataTransferObjects;
using KesselRun.Web.Api.Messaging.Queries;
using KesselRunFramework.AspNet.Infrastructure;
using KesselRunFramework.AspNet.Infrastructure.Controllers;
using KesselRunFramework.AspNet.Infrastructure.Extensions;
using KesselRunFramework.AspNet.Infrastructure.Invariants;
using KesselRunFramework.AspNet.Response;
using KesselRunFramework.Core.Cqrs.Queries;
using KesselRunFramework.Core.Infrastructure.Messaging;
using KesselRunFramework.Core.Infrastructure.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace KesselRun.Web.Api.Controllers.V1._0
{
    [ApiVersion(Swagger.Versions.v1_0)]
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
        [MapToApiVersion(Swagger.Versions.v1_0)]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetColors()
        {
            var colors = await _queryHandler.HandleAsync(new GetColorsQuery(), CancellationToken.None);

            return colors.Match(
                result => OkResponse(colors.LeftOrDefault()), 
                error => colors.RightOrDefault().ToUnprocessableRequestResult()                
                );
        }
    }
}
