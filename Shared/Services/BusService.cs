using MassTransit;
using Shared.Interfaces.Services;

namespace Shared.Services
{
    public class BusService : IBusService
    {
        private readonly Lazy<IPublishEndpoint> _publishEndpoint;
        private readonly Lazy<ISendEndpointProvider> _sendEndpointProvider;

        public BusService(Lazy<IPublishEndpoint> publishEndpoint, Lazy<ISendEndpointProvider> sendEndpointProvider)
        {
            _publishEndpoint = publishEndpoint;
            _sendEndpointProvider = sendEndpointProvider;
        }

        public Task Publish<TMessage>(TMessage message)
            => _publishEndpoint.Value.Publish(message);

        public async Task Send<TMessage>(string queue, TMessage message)
        {
            var endpoint = await _sendEndpointProvider.Value.GetSendEndpoint(new Uri($"queue:{queue}"));
            await endpoint.Send(message);
        }
    }
}
