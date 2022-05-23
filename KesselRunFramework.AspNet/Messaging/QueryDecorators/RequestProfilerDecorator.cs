using KesselRunFramework.AspNet.Infrastructure.Logging;
using KesselRunFramework.Core.Cqrs.Queries;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace KesselRunFramework.AspNet.Messaging.QueryDecorators
{
    public class RequestProfilerDecorator<TQuery, TResponse> : IQueryHandler<TQuery, TResponse>
            where TQuery : IQuery<TResponse>
            where TResponse : class
    {
        private readonly ILogger _logger;
        private readonly IQueryHandler<TQuery, TResponse> _decoratee;

        public RequestProfilerDecorator(ILogger logger, IQueryHandler<TQuery, TResponse> decoratee)
        {
            _logger = logger;
            _decoratee = decoratee;
        }
        

        public async Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();

            var result = await _decoratee.HandleAsync(query, cancellationToken);

            stopwatch.Stop();

            _logger.TraceMessageProfiling(stopwatch.ElapsedMilliseconds);

            return result;
        }
    }
}
