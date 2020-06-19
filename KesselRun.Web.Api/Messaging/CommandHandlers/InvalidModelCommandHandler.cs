using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KesselRun.Web.Api.HttpClients;
using KesselRun.Web.Api.Messaging.Commands;
using KesselRunFramework.Core.Infrastructure.Http;
using MediatR;

namespace KesselRun.Web.Api.Messaging.CommandHandlers
{
    public class InvalidModelCommandHandler : IRequestHandler<InvalidModelCommand, string>
    {
        private readonly InvalidModelClient _invalidModelClient;

        public InvalidModelCommandHandler(ITypedClientResolver typedClientResolver)
        {
            _invalidModelClient = typedClientResolver.GetTypedClient<InvalidModelClient>();
        }

        public async Task<string> Handle(InvalidModelCommand request, CancellationToken cancellationToken)
        {

            var result = await _invalidModelClient.GetPayload(request.Id.Value, request.Name,cancellationToken);

            return result.Match(
                c => result.LeftOrDefault(),
                e => result.RightOrDefault().Extensions.Select(v => v.Value.ToString()).Aggregate((curr,agg) => agg += curr)
                );
        }
    }
}
