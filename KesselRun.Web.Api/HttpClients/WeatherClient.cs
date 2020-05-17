using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace KesselRun.Web.Api.HttpClients
{
    public class WeatherClient : TypedClientBase
    {
        public string City { get; set; }
        public string Units { get; set; }

        public WeatherClient(HttpClient httpClient)
            : base(httpClient)
        {
            httpClient.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/weather");
            UriBuilder = new UriBuilder(HttpClient.BaseAddress);
            QueryStringParams = HttpUtility.ParseQueryString(UriBuilder.Query);
            QueryStringParams["appid"] = "";
        }

        public async Task<string> GetWeather(CancellationToken cancellationToken)
        {
            QueryStringParams["q"] = City;
            QueryStringParams["units"] = Units;

            UriBuilder.Query = QueryStringParams.ToString();

            string weather = "";

            using (var response = await HttpClient.GetAsync(UriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                var stream = await response.Content.ReadAsStreamAsync();
                response.EnsureSuccessStatusCode();

                using (var sr = new StreamReader(stream))
                {
                    weather = sr.ReadToEnd();
                }
            }

            return weather;
        }
    }
}
