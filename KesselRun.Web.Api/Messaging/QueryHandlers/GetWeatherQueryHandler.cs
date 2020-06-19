using System.Threading;
using System.Threading.Tasks;
using KesselRun.Business.DataTransferObjects.Weather;
using KesselRun.Web.Api.HttpClients;
using KesselRun.Web.Api.Messaging.Queries;
using KesselRunFramework.Core.Infrastructure.Http;
using MediatR;

namespace KesselRun.Web.Api.Messaging.QueryHandlers
{
    public class GetWeatherQueryHandler : IRequestHandler<GetWeatherQuery, WeatherDto>
    {
        private readonly WeatherClient _weatherClient;

        public GetWeatherQueryHandler(ITypedClientResolver typedClientResolver)
        {
            _weatherClient = typedClientResolver.GetTypedClient<WeatherClient>();
        }

        public async Task<WeatherDto> Handle(GetWeatherQuery request, CancellationToken cancellationToken)
        {
            _weatherClient.City = request.City;
            _weatherClient.Units = request.Units;

            return await _weatherClient.GetWeather(cancellationToken);
        }
    }
}
