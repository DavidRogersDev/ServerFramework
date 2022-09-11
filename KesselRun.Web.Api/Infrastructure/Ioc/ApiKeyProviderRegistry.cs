using KesselRun.Web.Api.HttpClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KesselRun.Web.Api.Infrastructure.Ioc
{
    public static class ApiKeyProviderRegistry
    {
        public static void AddApiKeyProvider(this IServiceCollection source, IConfiguration configuration)
        {
            var apiKeyProvider = new ApiKeyProvider
            {
                OpenMovieDatabaseApiKey = configuration["omdbapi"],
                OpenWeatherApiKey = configuration["openweathermap"]
            };

            source.AddSingleton(sp => apiKeyProvider);
        }
    }
}
