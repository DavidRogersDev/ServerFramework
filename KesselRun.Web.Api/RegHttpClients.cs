using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace KesselRun.Web.Api
{
    public static class RegHttpClients
    {
        public static void RegisterAllClients(this IServiceCollection services, IEnumerable<Type> types)
        {
            var addHttpClientMethod = typeof(HttpClientFactoryServiceCollectionExtensions).GetMethods().Single(
                m =>
                    m.Name == "AddHttpClient" &&
                    m.GetGenericArguments().Length == 1 &&
                    m.GetParameters().Length == 1 &&
                    m.GetParameters()[0].ParameterType == typeof(IServiceCollection)
                    ); 

            foreach (var type in types)
            {
                var genericMethod = addHttpClientMethod.MakeGenericMethod(type);
                var httpClientBuilder = (IHttpClientBuilder)genericMethod.Invoke(null, new []{ services});
                
                httpClientBuilder.ConfigurePrimaryHttpMessageHandler(handler => new HttpClientHandler
                {
                    AutomaticDecompression = System.Net.DecompressionMethods.GZip
                });
            }
        }
    }
}
