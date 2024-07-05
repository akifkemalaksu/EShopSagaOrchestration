using MassTransit;
using Shared.Interfaces.Commands;
using Shared.Interfaces.Events;

namespace Order.API.Consumers
{
    public class OrderRequestCompletedEventConsumer : IConsumer<IOrderRequestCompletedEvent>
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public OrderRequestCompletedEventConsumer(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task Consume(ConsumeContext<IOrderRequestCompletedEvent> context)
        {
            return _commandDispatcher.Dispatch(context.Message);
        }
    }
}
