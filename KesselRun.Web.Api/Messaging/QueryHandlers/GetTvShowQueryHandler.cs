using KesselRun.Web.Api.HttpClients;
using KesselRun.Web.Api.Messaging.Queries;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using KesselRunFramework.Core.Infrastructure.Http;

namespace KesselRun.Web.Api.Messaging.QueryHandlers
{
    public class GetTvShowQueryHandler : IRequestHandler<GetTvShowQuery, string>
    {
        private readonly OpenMovieDbClient _openMovieDbClient;

        public GetTvShowQueryHandler(ITypedClientResolver typedClientResolver)
        {
            _openMovieDbClient = typedClientResolver.GetTypedClient<OpenMovieDbClient>();
        }

        public async Task<string> Handle(GetTvShowQuery request, CancellationToken cancellationToken)
        { 
            return await _openMovieDbClient.GetShow(request.Title, request.Season, cancellationToken);
        }
    }
}
