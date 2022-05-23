namespace KesselRunFramework.Core.Cqrs.Commands
{
    public interface ICommandProcessor
    {
        TResult Execute<TResult>(ICommand<TResult> command);
    }
}
