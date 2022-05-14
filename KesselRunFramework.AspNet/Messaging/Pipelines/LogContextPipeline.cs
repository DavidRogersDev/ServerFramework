using System.Threading;
using System.Threading.Tasks;
using KesselRunFramework.AspNet.Infrastructure.Invariants;
using MediatR;
using Serilog.Context;

namespace KesselRunFramework.AspNet.Messaging.Pipelines
{
    public class LogContextPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            using (LogContext.PushProperty(Logging.LogContexts.RequestTypeProperty, typeof(TRequest).Name))
            {
                return await next();
            }
        }
    }
}
