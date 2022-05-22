using KesselRun.Web.Api.HttpClients;
using KesselRun.Web.Api.Messaging.Queries;
using System.Threading;
using System.Threading.Tasks;
using KesselRun.Business.DataTransferObjects.Media;
using KesselRunFramework.Core.Infrastructure.Http;
using KesselRunFramework.Core.Infrastructure.Validation;
using Microsoft.AspNetCore.Mvc;
using KesselRunFramework.Core.Cqrs.Queries;

namespace KesselRun.Web.Api.Messaging.QueryHandlers
{
    public class GetTvShowQueryHandler : IQueryHandler<GetTvShowQuery, Either<TvShow, ProblemDetails>>
    {
        private readonly OpenMovieDbClient _openMovieDbClient;

        public GetTvShowQueryHandler(ITypedClientResolver typedClientResolver)
        {
            _openMovieDbClient = typedClientResolver.GetTypedClient<OpenMovieDbClient>();
        }

        
        public async Task<Either<TvShow, ProblemDetails>> HandleAsync(GetTvShowQuery query, CancellationToken cancellationToken)
        {
            return await _openMovieDbClient.GetShow(query.Title, query.Season, cancellationToken);
        }
    }
}
