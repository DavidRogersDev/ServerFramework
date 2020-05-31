using KesselRun.Business.DataTransferObjects.Weather;
using MediatR;

namespace KesselRun.Web.Api.Messaging.Queries
{
    public class GetWeatherQuery : IRequest<WeatherDto>
    {
        public string City { get; set; }
        public string Units { get; set; }
    }
}
