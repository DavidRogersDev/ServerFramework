using System.Threading;
using System.Threading.Tasks;

namespace KesselRun.Web.Api.New
{    
    public interface ICommandHandler<in TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        Task<TResponse> ExecuteAsync(TCommand command, CancellationToken cancellationToken);
    }
}
