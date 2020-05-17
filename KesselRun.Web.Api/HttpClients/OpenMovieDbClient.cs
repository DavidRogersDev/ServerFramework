using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using KesselRunFramework.AspNet.Infrastructure.HttpClient;

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


        public async Task<string> GetShow(string title, int season, CancellationToken cancellationToken)
        {
            QueryStringParams["t"] = title;
            QueryStringParams["Season"] = season.ToString();

            UriBuilder.Query = QueryStringParams.ToString();

            string repos = "";

            using (var response = await HttpClient.GetAsync(UriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                var stream = await response.Content.ReadAsStreamAsync();
                response.EnsureSuccessStatusCode();

                using (var sr = new StreamReader(stream))
                {
                    repos = sr.ReadToEnd();
                }
            }

            return repos;
        }
    }
}
