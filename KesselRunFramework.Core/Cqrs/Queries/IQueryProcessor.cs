using System.Threading;
using System.Threading.Tasks;

namespace KesselRunFramework.Core.Cqrs.Queries
{
    public interface IQueryProcessor
    {
        Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken);
    }
}
