using KesselRun.Web.Api.Messaging.Commands;
using KesselRunFramework.Core.Cqrs.Commands;
using System.Threading;
using System.Threading.Tasks;



namespace KesselRun.Web.Api.Messaging.CommandHandlers
{
    public class AddColorCommandHandler : ICommandHandler<AddColorCommand, int>
    {
        public async Task<int> ExecuteAsync(AddColorCommand command, CancellationToken cancellationToken)
        {
            return await Task.FromResult(1);
        }
    }
}
