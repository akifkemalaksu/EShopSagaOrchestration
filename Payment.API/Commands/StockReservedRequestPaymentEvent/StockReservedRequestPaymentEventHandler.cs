using MassTransit;
using Shared.Events;
using Shared.Interfaces.Commands;
using Shared.Interfaces.Events;
using Shared.Interfaces.Services;

namespace Payment.API.Commands.StockReservedRequestPaymentEvent
{
    public class StockReservedRequestPaymentEventHandler : ICommandHandler<IStockReservedRequestPaymentEvent>
    {
        private readonly ILogger<StockReservedRequestPaymentEventHandler> _logger;
        private readonly IBusService _busService;

        public StockReservedRequestPaymentEventHandler(ILogger<StockReservedRequestPaymentEventHandler> logger, IBusService busService)
        {
            _logger = logger;
            _busService = busService;
        }

        public Task Handle(IStockReservedRequestPaymentEvent command, CancellationToken cancellationToken)
        {
            var balance = 3000m;

            if (balance > command.Payment.TotalPrice)
            {
                _logger.LogInformation($"{command.Payment.TotalPrice} TL was withdrawn from credit card for user id: {command.BuyerId}");

                var paymentSucceedEvent = new PaymentCompletedEvent(command.CorrelationId);

                _busService.Publish(paymentSucceedEvent);
            }
            else
            {
                _logger.LogInformation($"{command.Payment.TotalPrice} TL was not withdrawn from credit card for user id: {command.BuyerId}");

                var paymentFailedEvent = new PaymentFailedEvent(command.CorrelationId)
                {
                    Reason = "Not enough balance.",
                    OrderItems = command.OrderItems
                };

                _busService.Publish(paymentFailedEvent);
            }

            return Task.CompletedTask;
        }
    }
}
