using KesselRun.Business.DataTransferObjects.Media;
using KesselRunFramework.Core.Cqrs.Queries;
using KesselRunFramework.Core.Infrastructure.Validation;
using Microsoft.AspNetCore.Mvc;

namespace KesselRun.Web.Api.Messaging.Queries
{
    public class GetTvShowQuery : IQuery<Either<TvShow, ProblemDetails>>
    {
        public int Season { get; set; }
        public string Title { get; set; }
    }
}
