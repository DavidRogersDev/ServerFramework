using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using KesselRunFramework.AspNet.Infrastructure.Logging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KesselRunFramework.AspNet.Messaging.Pipelines
{
    public class OperationProfilingPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _logger;

        public OperationProfilingPipeline(ILogger logger)
        {
            _logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var stopwatch = Stopwatch.StartNew();

            var result = await next();

            stopwatch.Stop();
            
            _logger.TraceMessageProfiling(stopwatch.ElapsedMilliseconds);

            return result;
        }
    }
}
