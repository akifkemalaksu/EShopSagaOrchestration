using MassTransit;
using Shared.Interfaces.Commands;
using Shared.Interfaces.Events;

namespace Stock.API.Consumers
{
    public class PaymentFailedStockRollbackEventConsumer : IConsumer<IPaymentFailedStockRollbackEvent>
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public PaymentFailedStockRollbackEventConsumer(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task Consume(ConsumeContext<IPaymentFailedStockRollbackEvent> context)
        {
            return _commandDispatcher.Dispatch(context.Message);
        }
    }
}
