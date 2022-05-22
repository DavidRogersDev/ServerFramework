using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using KesselRunFramework.AspNet.Infrastructure.HttpClient;
using KesselRunFramework.Core.Infrastructure.Errors;
using KesselRunFramework.Core.Infrastructure.Http;
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
        }

        public async Task<Either<string, ProblemDetails>> GetPayload(int id, string name, CancellationToken cancellationToken)
        {
            string weather = "";
            string errors = "";
            HttpStatusCode statusCode = default(HttpStatusCode);

            var form = new FormUrlEncodedContent(new []
            { 
                new KeyValuePair<string, string>("id", id.ToString()), 
                new KeyValuePair<string, string>("name", name),
            });

            try
            {
                using (var response = await HttpClient.PostAsync(HttpClient.BaseAddress, form, cancellationToken))
                {
                    statusCode = response.StatusCode;

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

                if (statusCode == HttpStatusCode.InternalServerError)
                {
                    var problemDetails = new ProblemDetails();
                    problemDetails.Extensions.Add("HttpClientException", "An error was thrown from an API being called.");
                    problemDetails.Title = ProblemDetailTitles.InternalServerError;
                    problemDetails.Type = ProblemDetailTypes.InternalServerError;
                    problemDetails.Status = (int)HttpStatusCode.InternalServerError;

                    return problemDetails;
                }

                if (statusCode == HttpStatusCode.Unauthorized)
                {
                    var problemDetails = new ProblemDetails();
                    problemDetails.Extensions.Add("UnauthorisedException", "A 401 Unauthorized response was received from an API being called.");
                    problemDetails.Title = ProblemDetailTitles.AuthorizationError;
                    problemDetails.Type = ProblemDetailTypes.Unauthorized;
                    problemDetails.Status = (int)HttpStatusCode.Unauthorized;

                    return problemDetails;
                }

            }

            return weather;
        }

    }
}
