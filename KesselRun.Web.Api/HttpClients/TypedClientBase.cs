using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace KesselRun.Web.Api.HttpClients
{
    public abstract class TypedClientBase
    {
        protected readonly HttpClient HttpClient;
        protected UriBuilder UriBuilder;
        protected NameValueCollection QueryStringParams;

        protected TypedClientBase(HttpClient httpClient)
        {
            HttpClient = httpClient;
            httpClient.Timeout = new System.TimeSpan(0, 0, 30);
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
            httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue(DecompressionMethods.GZip.ToString().ToLower()));
        }
    }
}
