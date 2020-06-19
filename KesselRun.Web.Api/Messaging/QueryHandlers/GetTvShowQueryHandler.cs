using KesselRun.Web.Api.HttpClients;
using KesselRun.Web.Api.Messaging.Queries;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using KesselRun.Business.DataTransferObjects.Media;
using KesselRunFramework.Core.Infrastructure.Http;
using KesselRunFramework.Core.Infrastructure.Validation;
using Microsoft.AspNetCore.Mvc;

namespace KesselRun.Web.Api.Messaging.QueryHandlers
{
    public class GetTvShowQueryHandler : IRequestHandler<GetTvShowQuery, Either<TvShow, ProblemDetails>>
    {
        private readonly OpenMovieDbClient _openMovieDbClient;

        public GetTvShowQueryHandler(ITypedClientResolver typedClientResolver)
        {
            _openMovieDbClient = typedClientResolver.GetTypedClient<OpenMovieDbClient>();
        }

        public async Task<Either<TvShow,ProblemDetails>> Handle(GetTvShowQuery request, CancellationToken cancellationToken)
        { 
            return await _openMovieDbClient.GetShow(request.Title, request.Season, cancellationToken);
        }
    }
}
