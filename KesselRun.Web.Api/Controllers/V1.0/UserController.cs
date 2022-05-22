using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using KesselRun.Business.DataTransferObjects;
using KesselRun.Web.Api.Messaging.Queries;
using KesselRunFramework.AspNet.Infrastructure;
using KesselRunFramework.AspNet.Infrastructure.Controllers;
using KesselRunFramework.AspNet.Infrastructure.Invariants;
using KesselRunFramework.AspNet.Response;
using KesselRunFramework.Core.Cqrs.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace KesselRun.Web.Api.Controllers.V1_0
{
    [ApiVersion(Swagger.Versions.v1_0)]
    [Route(AspNet.Mvc.DefaultControllerTemplate)]
    [Produces(MediaTypeNames.Application.Json)]

    public class UserController : AppApiController
    {
        private readonly IQueryHandler<GetUserQuery, UserPayloadDto> _queryHandler;

        public UserController(ICurrentUser currentUser, ILogger logger, IQueryHandler<GetUserQuery, UserPayloadDto> queryHandler) 
            : base(currentUser)
        {
            _queryHandler = queryHandler;
        }

        [HttpGet]
        [Route(AspNet.Mvc.IdAction)]
        [MapToApiVersion(Swagger.Versions.v1_0)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _queryHandler.HandleAsync(new GetUserQuery {UserId = id}, CancellationToken.None);

            return OkResponse(user);
        }
    }
}
