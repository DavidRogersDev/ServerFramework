using System;
using KesselRunFramework.AspNet.Infrastructure.Logging;
using Serilog;
using Serilog.Configuration;

namespace KesselRunFramework.AspNet.Infrastructure.Extensions
{
    public static class EnvironmentLoggerConfigurationExtensions
    {
        /// <summary>
        /// Enrich log events with a hash of the message template to uniquely identify the different event types.
        /// </summary>
        /// <param name="enrichmentConfiguration">The enrichment configuration.</param>
        /// <returns>Type: LoggerConfiguration. The LoggerConfiguration object resulting from the logger enrichment.</returns>
        /// <exception cref="System.ArgumentNullException">enrichmentConfiguration</exception>
        public static LoggerConfiguration WithEventType(this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
            return enrichmentConfiguration.With<EventTypeEnricher>();
        }
    }
}
