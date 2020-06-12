using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using KesselRun.Business.DataTransferObjects.Media;
using KesselRunFramework.AspNet.Infrastructure.HttpClient;
using KesselRunFramework.Core.Infrastructure.Extensions;
using KesselRunFramework.Core.Infrastructure.Http;
using KesselRunFramework.Core.Infrastructure.Validation;
using Microsoft.AspNetCore.Mvc;

namespace KesselRun.Web.Api.HttpClients
{
    public class OpenMovieDbClient : TypedClientBase, ITypedHttpClient
    {
        public OpenMovieDbClient(HttpClient httpClient)
            : base(httpClient)
        {
            HttpClient.BaseAddress = new Uri("https://www.omdbapi.com");
            UriBuilder = new UriBuilder(HttpClient.BaseAddress);
            QueryStringParams = HttpUtility.ParseQueryString(UriBuilder.Query);
            QueryStringParams["apikey"] = "";
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
                    return stream.ReadAndDeserializeFromJson<TvShow>();
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
