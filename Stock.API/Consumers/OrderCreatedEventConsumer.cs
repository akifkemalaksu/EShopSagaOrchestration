using MassTransit;
using Shared.Interfaces.Commands;
using Shared.Interfaces.Events;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<IOrderCreatedEvent>
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public OrderCreatedEventConsumer(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public async Task Consume(ConsumeContext<IOrderCreatedEvent> context)
        {
            await _commandDispatcher.Dispatch(context.Message);
        }
    }
}
