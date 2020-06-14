using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using KesselRunFramework.AspNet.Infrastructure.HttpClient;
using KesselRunFramework.Core.Infrastructure.Errors;
using KesselRunFramework.Core.Infrastructure.Logging;
using KesselRunFramework.Core.Infrastructure.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KesselRun.Web.Api.HttpClients
{
    public class InvalidModelClient : TypedClientBase, ITypedHttpClient
    {
        private readonly ILogger _logger;

        public InvalidModelClient(ILogger logger, HttpClient httpClient)
        : base(httpClient)
        {
            _logger = logger;
            HttpClient.BaseAddress = new Uri("https://localhost:44356/WeatherForecast");
            //UriBuilder = new UriBuilder(HttpClient.BaseAddress);
            //UriBuilder = new UriBuilder(HttpClient.BaseAddress);
            //QueryStringParams = HttpUtility.ParseQueryString(UriBuilder.Query);

        }

        public async Task<Either<string, ProblemDetails>> GetPayload(int id, string name, CancellationToken cancellationToken)
        {
            string weather = "";
            string errors = "";
            ProblemDetails problemDetails = null;
            //QueryStringParams["Id"] = id.ToString();
            //QueryStringParams["Name"] = name;

            //UriBuilder.Query = QueryStringParams.ToString();

            var form = new FormUrlEncodedContent(new []
            { 
                new KeyValuePair<string, string>("id", id.ToString()), 
                new KeyValuePair<string, string>("name", name),
            });

            try
            {
                using (var response = await HttpClient.PostAsync(HttpClient.BaseAddress, form, cancellationToken))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            errors = await response.Content.ReadAsStringAsync();
                            return System.Text.Json.JsonSerializer.Deserialize<ProblemDetails>(errors);
                        }
                    }

                    response.EnsureSuccessStatusCode();

                    weather = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(EventIDs.EventIdHttpClient,
                    exception,
                    MessageTemplates.HttpClientGet,
                    UriBuilder.Uri.AbsolutePath
                );
            }

            return weather;
        }

    }
}
