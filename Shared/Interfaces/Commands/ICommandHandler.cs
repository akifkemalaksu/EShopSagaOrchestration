namespace Shared.Interfaces.Commands
{
    public interface ICommandHandler<in TCommand, TCommandResult>
    {
        Task<TCommandResult> Handle(TCommand command, CancellationToken cancellationToken);
    }

    public interface ICommandHandler<in TCommand>
    {
        Task Handle(TCommand command, CancellationToken cancellationToken);
    }
}
