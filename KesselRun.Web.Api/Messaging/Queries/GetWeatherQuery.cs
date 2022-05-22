using KesselRun.Business.DataTransferObjects.Weather;
using KesselRunFramework.Core.Cqrs.Queries;

namespace KesselRun.Web.Api.Messaging.Queries
{
    public class GetWeatherQuery : IQuery<WeatherDto>
    {
        public string City { get; set; }
        public string Units { get; set; }
    }
}
