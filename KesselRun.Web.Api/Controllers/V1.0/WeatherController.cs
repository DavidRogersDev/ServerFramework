using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using KesselRun.Business.DataTransferObjects;
using KesselRun.Business.DataTransferObjects.Weather;
using KesselRun.Web.Api.HttpClients;
using KesselRun.Web.Api.Messaging.Commands;
using KesselRun.Web.Api.Messaging.Queries;
using KesselRunFramework.AspNet.Infrastructure;
using KesselRunFramework.AspNet.Infrastructure.Controllers;
using KesselRunFramework.AspNet.Infrastructure.Invariants;
using KesselRunFramework.AspNet.Response;
using KesselRunFramework.Core.Cqrs.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KesselRun.Web.Api.Controllers.V1._0
{
    [ApiVersion(Swagger.Versions.v1_0)]
    [Route(AspNet.Mvc.DefaultControllerTemplate)]
    [Produces(MediaTypeNames.Application.Json)]
    public class WeatherController : AppApiController
    {
        private readonly RetryTestClient _retryTestClient;
        private readonly IQueryHandler<InvalidModelCommand, string> _invalidModelQueryHandler;
        private readonly IQueryHandler<GetWeatherQuery, WeatherDto> _queryHandler;

        public WeatherController(
            ICurrentUser currentUser,
            IQueryHandler<GetWeatherQuery, WeatherDto> queryHandler,
            RetryTestClient retryTestClient,
            IQueryHandler<InvalidModelCommand, string> invalidModelQueryHandler)
            : base(currentUser)
        {
            _retryTestClient = retryTestClient;
            _invalidModelQueryHandler = invalidModelQueryHandler;
            _queryHandler = queryHandler;
        }

        [HttpGet]
        [Route(AspNet.Mvc.ActionTemplate)]
        [MapToApiVersion(Swagger.Versions.v1_0)]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetWeather([FromQuery]WeatherPayloadDto weatherPayloadDto)
        {
            var weather = await _queryHandler.HandleAsync(new GetWeatherQuery { City = weatherPayloadDto.City, Units = weatherPayloadDto.Units }, CancellationToken.None);

            return OkResponse(weather);
        }

        [HttpGet]
        [Route(AspNet.Mvc.ActionTemplate)]
        [MapToApiVersion(Swagger.Versions.v1_0)]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RetryTest()
        {
            var weather = await _retryTestClient.GetTestPayload();

            return OkResponse(weather);
        }

        [HttpPost]
        [Route(AspNet.Mvc.ActionTemplate)]
        [MapToApiVersion(Swagger.Versions.v1_0)]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> TestInvalidModel(int id, string name)
        {
            var send = await _invalidModelQueryHandler.HandleAsync(new InvalidModelCommand
            {
                Id = id,
                Name = name
            }, CancellationToken.None);

            return OkResponse(send);
        }
    }
}
