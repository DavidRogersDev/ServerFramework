using KesselRun.Business.DataTransferObjects.Media;
using KesselRunFramework.Core.Infrastructure.Validation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KesselRun.Web.Api.Messaging.Queries
{
    public class GetTvShowQuery : IRequest<Either<TvShow, ProblemDetails>>
    {
        public int Season { get; set; }
        public string Title { get; set; }
    }
}
