using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using KesselRun.Business.DataTransferObjects;
using KesselRun.Business.DataTransferObjects.Weather;
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
        private readonly IQueryHandler<GetWeatherQuery, WeatherDto> _queryHandler;

        public WeatherController(ICurrentUser currentUser, IQueryHandler<GetWeatherQuery, WeatherDto> queryHandler
            ) : base(currentUser)
        {
            _queryHandler = queryHandler;
        }

        [HttpGet]
        [Route(AspNet.Mvc.ActionTemplate)]
        [MapToApiVersion(Swagger.Versions.v1_0)]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetWeather([FromQuery] WeatherPayloadDto weatherPayloadDto)
        {
            var weather = await _queryHandler.HandleAsync(new GetWeatherQuery { City = weatherPayloadDto.City, Units = weatherPayloadDto.Units }, CancellationToken.None);

            return OkResponse(weather);
        }

    }
}
