using System.Threading;
using System.Threading.Tasks;
using KesselRun.Web.Api.HttpClients;
using KesselRun.Web.Api.Messaging.Queries;
using KesselRunFramework.Core.Infrastructure.Http;
using MediatR;

namespace KesselRun.Web.Api.Messaging.QueryHandlers
{
    public class GetWeatherQueryHandler : IRequestHandler<GetWeatherQuery, string>
    {
        private readonly WeatherClient _weatherClient;

        public GetWeatherQueryHandler(ITypedClientResolver typedClientResolver)
        {
            _weatherClient = typedClientResolver.GetTypedClient<WeatherClient>();
        }

        public async Task<string> Handle(GetWeatherQuery request, CancellationToken cancellationToken)
        {
            _weatherClient.City = "Sydney, AU";
            _weatherClient.Units = "imperial";

            return await _weatherClient.GetWeather(cancellationToken);
        }
    }
}
