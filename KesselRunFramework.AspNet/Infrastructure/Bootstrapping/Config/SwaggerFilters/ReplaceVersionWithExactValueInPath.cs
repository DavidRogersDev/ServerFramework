using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Config.SwaggerFilters
{
    public class ReplaceVersionWithExactValueInPath : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var newPaths = new OpenApiPaths();

            foreach (var swaggerDocPath in swaggerDoc.Paths)
            {
                //  swaggerDoc.Info.Version gets set by c.SwaggerDoc in AddSwaggerGen 
                newPaths.Add(
                    swaggerDocPath.Key.Replace("v{version}", string.Concat("V", swaggerDoc.Info.Version)),
                    swaggerDocPath.Value
                    );
            }

            swaggerDoc.Paths = newPaths;
        }
    }
}
