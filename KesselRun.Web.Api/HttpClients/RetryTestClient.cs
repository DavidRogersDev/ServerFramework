using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using KesselRunFramework.AspNet.Infrastructure.HttpClient;

namespace KesselRun.Web.Api.HttpClients
{
    public class RetryTestClient : TypedClientBase, ITypedHttpClient
    {
        public RetryTestClient(HttpClient httpClient) 
            : base(httpClient)
        {
        }

        public async Task<string> GetTestPayload()
        {
            string weather = "";

            using (var response = await HttpClient.GetAsync(new Uri("https://localhost:44356/weatherforecast"), HttpCompletionOption.ResponseHeadersRead))
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
