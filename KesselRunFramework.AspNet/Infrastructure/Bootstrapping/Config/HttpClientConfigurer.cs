using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Config
{
    public static class HttpClientConfigurer
    {
        public static void RegisterTypedHttpClients(this IServiceCollection services, IEnumerable<Type> types)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (types == null) throw new ArgumentNullException(nameof(types));

            var typesArray = types as Type[] ?? types.ToArray();

            if (typesArray.Any())
            {
                var noOpPolicy = Policy.NoOpAsync().AsAsyncPolicy<HttpResponseMessage>();

                var retryPolicy = HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(new[]
                    {
                        TimeSpan.FromSeconds(2),
                        TimeSpan.FromSeconds(4),
                        TimeSpan.FromSeconds(8)
                    });

                var addHttpClientMethod = typeof(HttpClientFactoryServiceCollectionExtensions).GetMethods()
                    .Single(
                        m =>
                            m.Name == "AddHttpClient" &&
                            m.GetGenericArguments().Length == 1 &&
                            m.GetParameters().Length == 1 &&
                            m.GetParameters()[0].ParameterType == typeof(IServiceCollection)
                    );

                for (int i = 0; i < typesArray.Length; i++)
                {
                    var genericMethod = addHttpClientMethod.MakeGenericMethod(typesArray[i]);

                    var httpClientBuilder = (IHttpClientBuilder)genericMethod.Invoke(null, new[] { services });

                    httpClientBuilder.ConfigurePrimaryHttpMessageHandler(handler => new HttpClientHandler
                    {
                        AutomaticDecompression = System.Net.DecompressionMethods.GZip
                    }).AddPolicyHandler(
                        request => request.Method == HttpMethod.Get 
                            ? retryPolicy 
                            : noOpPolicy
                        );
                }
            }
        }
    }
}
