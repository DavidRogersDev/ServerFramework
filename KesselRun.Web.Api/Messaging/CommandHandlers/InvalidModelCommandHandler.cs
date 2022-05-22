using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KesselRun.Web.Api.HttpClients;
using KesselRun.Web.Api.Messaging.Commands;
using KesselRunFramework.Core.Cqrs.Queries;
using KesselRunFramework.Core.Infrastructure.Http;

namespace KesselRun.Web.Api.Messaging.CommandHandlers
{
    public class InvalidModelCommandHandler : IQueryHandler<InvalidModelCommand, string>
    {
        private readonly InvalidModelClient _invalidModelClient;

        public InvalidModelCommandHandler(ITypedClientResolver typedClientResolver)
        {
            _invalidModelClient = typedClientResolver.GetTypedClient<InvalidModelClient>();
        }


        public async Task<string> HandleAsync(InvalidModelCommand query, CancellationToken cancellationToken)
        {
            var result = await _invalidModelClient.GetPayload(query.Id.Value, query.Name, cancellationToken);

            return result.Match(
                c => result.LeftOrDefault(),
                e => result.RightOrDefault().Extensions.Select(v => v.Value.ToString()).Aggregate((curr, agg) => agg += curr)
                );
        }
    }
}
