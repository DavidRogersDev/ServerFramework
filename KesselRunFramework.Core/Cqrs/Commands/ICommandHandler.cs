using System.Threading;
using System.Threading.Tasks;

namespace KesselRunFramework.Core.Cqrs.Commands
{
    public interface ICommandHandler<in TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        Task<TResponse> ExecuteAsync(TCommand command, CancellationToken cancellationToken);
    }
}
