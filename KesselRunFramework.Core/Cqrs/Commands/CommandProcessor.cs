using SimpleInjector;

namespace KesselRunFramework.Core.Cqrs.Commands
{
    public class CommandProcessor : ICommandProcessor
    {
        private readonly Container _container;

        public CommandProcessor(Container container)
        {
            _container = container;
        }

        public TResult Execute<TResult>(ICommand<TResult> command)
        {
            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));

            dynamic queryHandler = _container.GetInstance(handlerType);

            return queryHandler.Handle((dynamic)command);
        }
    }
}
