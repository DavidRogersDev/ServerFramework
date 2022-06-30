using KesselRun.Web.Api.Messaging.Queries;
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
using KesselRun.Business.DataTransferObjects;

namespace KesselRun.Web.Api.Controllers.V1._1
{
    [ApiVersion(Swagger.Versions.v1_1)]
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
        [MapToApiVersion(Swagger.Versions.v1_1)]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetColors()
        {
            var colors = await _mediator.Send(new GetColorsQuery());

            return colors.Match(
                result => OkResponse(colors.LeftOrDefault()),
                error => BadRequestResponse(
                    string.Empty,
                    OperationOutcome.ValidationFailOutcome(colors.RightOrDefault().ValidationFailures)
                    )
                );
        }
    }
}
