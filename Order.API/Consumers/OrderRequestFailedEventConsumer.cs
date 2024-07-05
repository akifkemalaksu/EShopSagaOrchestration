using MassTransit;
using Shared.Interfaces.Commands;
using Shared.Interfaces.Events;

namespace Order.API.Consumers
{
    public class OrderRequestFailedEventConsumer : IConsumer<IOrderRequestFailedEvent>
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public OrderRequestFailedEventConsumer(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task Consume(ConsumeContext<IOrderRequestFailedEvent> context)
        {
            return _commandDispatcher.Dispatch(context.Message);
        }
    }
}
