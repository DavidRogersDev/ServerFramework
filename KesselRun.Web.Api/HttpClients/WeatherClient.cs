using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using KesselRun.Business.DataTransferObjects.Weather;
using KesselRunFramework.AspNet.Infrastructure.HttpClient;
using KesselRunFramework.Core.Infrastructure.Errors;
using KesselRunFramework.Core.Infrastructure.Logging;
using Microsoft.Extensions.Logging;

namespace KesselRun.Web.Api.HttpClients
{
    public class WeatherClient : TypedClientBase, ITypedHttpClient
    {
        private readonly ILogger _logger;
        readonly ApiKeyProvider _apiKeyProvider;
        public string City { get; set; }
        public string Units { get; set; }

        public WeatherClient(ILogger logger, HttpClient httpClient, ApiKeyProvider apiKeyProvider)
            : base(httpClient)
        {
            _apiKeyProvider = apiKeyProvider;
            _logger = logger;
            HttpClient.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/weather");
            UriBuilder = new UriBuilder(HttpClient.BaseAddress);
            QueryStringParams = HttpUtility.ParseQueryString(UriBuilder.Query);
            QueryStringParams["appid"] = _apiKeyProvider.OpenWeatherApiKey; // your app id goes here.
        }

        public async Task<WeatherDto> GetWeather(CancellationToken cancellationToken)
        {
            QueryStringParams["q"] = City;
            QueryStringParams["units"] = Units;

            UriBuilder.Query = QueryStringParams.ToString();

            WeatherDto weather = null;

            try
            {
                using (var response = await HttpClient.GetAsync(UriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
                        {
                            var errorStream = await response.Content.ReadAsStreamAsync(cancellationToken);

                            // Do something with errors here. The following code is just a guide and incomplete

                            using (var streamReader = new StreamReader(errorStream, Encoding.UTF8, true))
                            {
                                //using (var jsonTextReader = new JsonTextReader(streamReader))
                                {
                                    //var jsonSerializer = new JsonSerializer();
                                    //'var validationErrors = await JsonSerializer.DeserializeAsync(errorStream);
                                }
                            }
                        }
                    }

                    response.EnsureSuccessStatusCode();

                    var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                    weather = await DeserializeAsync<WeatherDto>(stream);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(EventIDs.EventIdHttpClient, 
                    exception,
                    MessageTemplates.HttpClientGet, 
                    UriBuilder.Uri.AbsolutePath
                    );
                
                throw;
            }

            return weather;
        }
    }
}
