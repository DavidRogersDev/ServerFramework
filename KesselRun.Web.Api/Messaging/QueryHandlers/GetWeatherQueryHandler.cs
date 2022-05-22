using System.Threading;
using System.Threading.Tasks;
using KesselRun.Business.DataTransferObjects.Weather;
using KesselRun.Web.Api.HttpClients;
using KesselRun.Web.Api.Messaging.Queries;
using KesselRunFramework.Core.Cqrs.Queries;
using KesselRunFramework.Core.Infrastructure.Http;

namespace KesselRun.Web.Api.Messaging.QueryHandlers
{
    public class GetWeatherQueryHandler : IQueryHandler<GetWeatherQuery, WeatherDto>
    {
        private readonly WeatherClient _weatherClient;

        public GetWeatherQueryHandler(ITypedClientResolver typedClientResolver)
        {
            _weatherClient = typedClientResolver.GetTypedClient<WeatherClient>();
        }

       

        public async Task<WeatherDto> HandleAsync(GetWeatherQuery query, CancellationToken cancellationToken)
        {
            _weatherClient.City = query.City;
            _weatherClient.Units = query.Units;

            return await _weatherClient.GetWeather(cancellationToken);
        }
    }
}
