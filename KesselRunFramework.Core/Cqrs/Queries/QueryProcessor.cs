using SimpleInjector;
using System.Threading;
using System.Threading.Tasks;

namespace KesselRunFramework.Core.Cqrs.Queries
{
    public sealed class QueryProcessor : IQueryProcessor
    {
        private readonly Container _container;

        public QueryProcessor(Container container)
        {
            _container = container;
        }

        public async Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

            dynamic queryHandler = _container.GetInstance(handlerType);

            return await queryHandler.HandleAsync((dynamic)query, cancellationToken);
        }
    }
}
