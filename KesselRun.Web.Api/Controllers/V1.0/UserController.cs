using System.Net.Mime;
using System.Threading.Tasks;
using KesselRun.Web.Api.Messaging.Queries;
using KesselRunFramework.AspNet.Infrastructure;
using KesselRunFramework.AspNet.Infrastructure.Controllers;
using KesselRunFramework.AspNet.Infrastructure.Invariants;
using KesselRunFramework.AspNet.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace KesselRun.Web.Api.Controllers.V1_0
{
    [ApiVersion(Swagger.Versions.v1_0)]
    [Route(AspNet.Mvc.DefaultControllerTemplate)]
    [Produces(MediaTypeNames.Application.Json)]

    public class UserController : AppApiMediatrController
    {
        public UserController(ICurrentUser currentUser, ILogger logger, IMediator mediator) 
            : base(currentUser, logger, mediator)
        {
        }

        [HttpGet]
        [Route(AspNet.Mvc.IdAction)]
        [MapToApiVersion(Swagger.Versions.v1_0)]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _mediator.Send(new GetUserQuery {UserId = id});

            return OkResponse(user);
        }
    }
}
