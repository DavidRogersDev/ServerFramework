using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Config.SwaggerFilters;
using KesselRunFramework.AspNet.Infrastructure.Invariants;
using KesselRunFramework.AspNet.Infrastructure.SwaggerFilters;
using KesselRunFramework.Core.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Config
{
    public static class VersioningExtensions
    {
        public static IServiceCollection AddAppApiVersioning(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddApiVersioning(o =>
            {
                o.DefaultApiVersion = new ApiVersion(StartUp.MajorVersion1, StartUp.MinorVersion0); // specify the default api version
                o.AssumeDefaultVersionWhenUnspecified = true; // assume that the caller wants the default version if they don't specify
                o.ReportApiVersions = true; // add headers in responses which show supported/deprecated versions
            });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services,
            IWebHostEnvironment hostingEnvironment, 
            IConfiguration configuration, 
            IList<OpenApiInfo> openApiInfos)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (hostingEnvironment == null) throw new ArgumentNullException(nameof(hostingEnvironment));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            if (hostingEnvironment.IsDevelopmentOrIsStaging())
            {
                services.AddSwaggerGen(c =>
                {
                    for (int i = 0; i < openApiInfos.Count; i++)
                    {
                        var name = string.Concat(Swagger.Versions.VersionPrefix, openApiInfos[i].Version);
                        c.SwaggerDoc(name, openApiInfos[i]);
                    }

                    c.OperationFilter<RemoveVersionFromParameterFilter>();
                    c.DocumentFilter<ReplaceVersionWithExactValueInPath>();

                    //  This predicate is used by convention to obviate the need to decorate relevant actions with an 
                    //  ApiExplorerSettings attribute (setting the GroupName). That attribute is required where there
                    //  are different versions of the same action i.e. same name.
                    c.DocInclusionPredicate((docName, apiDesc) =>
                    {
                        if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

                        var versions = methodInfo.DeclaringType
                            .GetCustomAttributes(true)
                            .OfType<ApiVersionAttribute>()
                            .SelectMany(attr => attr.Versions);

                        var maps = apiDesc.CustomAttributes()
                            .OfType<MapToApiVersionAttribute>()
                            .SelectMany(attr => attr.Versions)
                            .ToArray();

                        return versions.Any(v => $"{Swagger.Versions.VersionPrefix}{v.ToString()}" == docName) 
                               && (maps.Length == 0 || maps.Any(v => $"{Swagger.Versions.VersionPrefix}{v.ToString()}" == docName));
                    });
                    
                    c.AddSecurityDefinition(Invariants.Identity.Bearer, new OpenApiSecurityScheme
                    {
                        Description = Swagger.SecurityDefinition.Description,
                        In = ParameterLocation.Header,
                        Name = Swagger.SecurityDefinition.Name,
                        Type = SecuritySchemeType.Http
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = Invariants.Identity.Bearer
                                },
                                Scheme = Invariants.Identity.Oauth2,
                                Name = Invariants.Identity.Bearer,
                                In = ParameterLocation.Header
                            },
                            new List<string>()
                        }
                    });

                    c.OperationFilter<SwaggerDefaultValuesFilter>();

                    c.DescribeAllParametersInCamelCase();
                    
                    //c.IncludeXmlComments(XmlCommentsFilePath);

                });
            }
            return services;
        }

        public static void UseSwaggerInDevAndStaging(this IApplicationBuilder app, IWebHostEnvironment hostingEnvironment, string[] versions)
        {
            if (hostingEnvironment.IsDevelopmentOrIsStaging())
            {
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                    {
                        for (int i = 0; i < versions.Length; i++)
                        {
                            c.SwaggerEndpoint(
                                Swagger.EndPoint.Url.FormatAs(string.Concat(Swagger.Versions.VersionPrefix, versions[i])),
                                Swagger.EndPoint.Name.FormatAs(string.Concat(Swagger.Versions.VersionPrefix, versions[i]))
                            );
                        }
                    });
            }
        }
    }
}
