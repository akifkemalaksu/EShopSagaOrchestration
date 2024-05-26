namespace Shared.Interfaces.Services
{
    public interface IBusService
    {
        Task Publish<TMessage>(TMessage message);

        Task Send<TMessage>(string queue, TMessage message);
    }
}
