using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Config
{
    public class JsonOptionsConfigurer
    {
        public static void ConfigureJsonOptions(JsonOptions jsonOptions)
        {
            jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        }
    }
}
