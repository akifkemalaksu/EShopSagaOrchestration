namespace Shared.Interfaces.Commands
{
    public interface ICommandDispatcher
    {
        Task<TCommandResult> Dispatch<TCommand, TCommandResult>(TCommand command, CancellationToken cancellationToken = default);

        Task Dispatch<TCommand>(TCommand command, CancellationToken cancellationToken = default);
    }
}
