﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Config.SwaggerFilters;
using KesselRunFramework.AspNet.Infrastructure.Invariants;
using KesselRunFramework.Core.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Config
{
    public static class VersioningExtensions
    {
        public static IServiceCollection AddAppApiVersioning(this IServiceCollection services, Action<ApiVersioningOptions> options = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddApiVersioning(options ?? (o =>
            {
                o.DefaultApiVersion = new ApiVersion(StartUpConfig.MajorVersion1, StartUpConfig.MinorVersion0); // specify the default api version
                o.AssumeDefaultVersionWhenUnspecified = true; // assume that the caller wants the default version if they don't specify
                o.ReportApiVersions = true; // add headers in responses which show supported/deprecated versions
            }));

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

                    // This line of code is to accommodate the fact that some of the classes have the same name (but are in different namespaces).
                    // Swashbuckle uses the class name as the SchemaId by default. If 2 classes have the same name, it goes 💣!
                    // This code includes the namespace, so there is no longer any conflict in SchemaIds.
                    c.CustomSchemaIds(x => x.FullName);

                    // This filter removed the "version" parameter from the parameters textboxes which are rendered.
                    // It is redundant and not a genuine parameter if using Urls for versioning.
                    c.OperationFilter<RemoveVersionFromParameterFilter>();
                    
                    // This filter adds the version to the paths rendered on the page so you can see what Version you are dealing with at a glance.
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

                        return versions.Any(v => $"{Swagger.Versions.VersionPrefix}{v.ToString()}" == docName) &&
                               (
                                    maps.Length == 0 ||
                                    maps.Any(v => $"{Swagger.Versions.VersionPrefix}{v.ToString()}" == docName)
                               );
                    });
                    
                    c.AddSecurityDefinition(Invariants.Identity.Bearer, new OpenApiSecurityScheme
                    {
                        Description = Swagger.SecurityDefinition.Description,
                        In = ParameterLocation.Header,
                        Name = Swagger.SecurityDefinition.Name,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = Invariants.Identity.Bearer
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
                            c.DefaultModelsExpandDepth(-1); // ◀ This prevents the rendering of the Models section at the bottom
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
