using Microsoft.AspNetCore.Http;
using Serilog;

namespace KesselRunFramework.AspNet.Infrastructure.Bootstrapping.Config
{
    public static class RequestLoggingConfigurer
    {
        public static void EnrichFromRequest(IDiagnosticContext diagnosticContext, HttpContext httpContext)
        {
            var request = httpContext.Request;

            // Set all the common properties available for every request
            diagnosticContext.Set(Microsoft.Net.Http.Headers.HeaderNames.Host, request.Host);
            diagnosticContext.Set(Invariants.AspNet.Request.Protocol, request.Protocol);
            diagnosticContext.Set(Invariants.AspNet.Request.Scheme, request.Scheme);

            // Only set it if available. You're not sending sensitive data in a querystring right?! 
            if (request.QueryString.HasValue)
            {
                diagnosticContext.Set("QueryString", request.QueryString.Value);
            }

            // Set the content-type of the Response at this point
            diagnosticContext.Set(Microsoft.Net.Http.Headers.HeaderNames.ContentType, httpContext.Response.ContentType);

            // Retrieve the IEndpointFeature selected for the request
            var endpoint = httpContext.GetEndpoint();

            if (!ReferenceEquals(endpoint, null))
            {
                diagnosticContext.Set(Invariants.AspNet.Request.EndpointName, endpoint.DisplayName);
            }
        }
    }
}
