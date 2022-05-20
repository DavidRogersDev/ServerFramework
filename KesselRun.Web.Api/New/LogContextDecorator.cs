using KesselRunFramework.AspNet.Infrastructure.Invariants;
using Serilog.Context;
using System.Threading;
using System.Threading.Tasks;

namespace KesselRun.Web.Api.New
{
    public class LogContextDecorator<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
        where TResponse : class
    {
        private readonly ICommandHandler<TCommand, TResponse> _decoratee;

        public LogContextDecorator(ICommandHandler<TCommand, TResponse> decoratee)
        {
            _decoratee = decoratee;
        }

        public async Task<TResponse> ExecuteAsync(TCommand command, CancellationToken cancellationToken)
        {
            using (LogContext.PushProperty(Logging.LogContexts.RequestTypeProperty, typeof(TCommand).Name))
            {
                return await _decoratee.ExecuteAsync(command, cancellationToken);
            }
        }
    }
}
