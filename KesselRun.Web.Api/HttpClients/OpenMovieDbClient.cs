using KesselRun.Business.DataTransferObjects.Media;
using KesselRunFramework.AspNet.Infrastructure.HttpClient;
using KesselRunFramework.Core.Infrastructure.Http;
using KesselRunFramework.Core.Infrastructure.Validation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace KesselRun.Web.Api.HttpClients
{
    public class OpenMovieDbClient : TypedClientBase, ITypedHttpClient
    {
        readonly ApiKeyProvider _apiKeyProvider;
        public OpenMovieDbClient(HttpClient httpClient, ApiKeyProvider apiKeyProvider)
            : base(httpClient)
        {
            _apiKeyProvider = apiKeyProvider;
            HttpClient.BaseAddress = new Uri("https://www.omdbapi.com");
            UriBuilder = new UriBuilder(HttpClient.BaseAddress);
            QueryStringParams = HttpUtility.ParseQueryString(UriBuilder.Query);
            QueryStringParams["apikey"] = _apiKeyProvider.OpenMovieDatabaseApiKey; // your api key goes here.
        }


        public async Task<Either<TvShow, ProblemDetails>> GetShow(string title, int season, CancellationToken cancellationToken)
        {
            QueryStringParams["t"] = title;
            QueryStringParams["Season"] = season.ToString();

            UriBuilder.Query = QueryStringParams.ToString();

            HttpStatusCode statusCode = default(HttpStatusCode);

            try
            {
                using (var response = await HttpClient.GetAsync(UriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
                {
                    statusCode = response.StatusCode;
                    response.EnsureSuccessStatusCode();

                    var stream = await response.Content.ReadAsStreamAsync();

                    return await DeserializeAsync<TvShow>(stream);
                }
            }
            catch (Exception exception)
            {
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
                    problemDetails.Status = (int) HttpStatusCode.Unauthorized;

                    return problemDetails;
                }

                throw;
            }
        }
    }
}
