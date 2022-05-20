using System.Threading;
using System.Threading.Tasks;

namespace KesselRun.Web.Api.New
{
    public interface IQueryHandler<in TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken);
    }
}