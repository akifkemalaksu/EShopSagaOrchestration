using MassTransit;
using Shared.Interfaces.Commands;
using Shared.Interfaces.Events;

namespace Payment.API.Consumers
{
    public class StockReservedRequestPaymentEventConsumer : IConsumer<IStockReservedRequestPaymentEvent>
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public StockReservedRequestPaymentEventConsumer(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task Consume(ConsumeContext<IStockReservedRequestPaymentEvent> context)
        {
            return _commandDispatcher.Dispatch(context.Message);
        }
    }
}
