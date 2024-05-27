using MassTransit;
using Shared.Interfaces.Commands;
using Shared.Interfaces.Events;

namespace Payment.API.Consumers
{
    public class StockReservedRequestPaymentConsumer : IConsumer<IStockReservedRequestPayment>
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public StockReservedRequestPaymentConsumer(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public async Task Consume(ConsumeContext<IStockReservedRequestPayment> context)
        {
            await _commandDispatcher.Dispatch(context.Message);
        }
    }
}
