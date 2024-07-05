using Order.API.Constants;
using Order.API.Contexts;
using Shared.Interfaces.Commands;
using Shared.Interfaces.Events;

namespace Order.API.Commands.OrderRequestFailedEvent
{
    public class OrderRequestFailedEventHandler : ICommandHandler<IOrderRequestFailedEvent>
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<OrderRequestFailedEventHandler> _logger;

        public OrderRequestFailedEventHandler(AppDbContext dbContext, ILogger<OrderRequestFailedEventHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(IOrderRequestFailedEvent command, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders.FindAsync(command.OrderId);

            if (order != null)
            {
                order.Status = OrderStatus.Fail;
                order.FailMessage = command.Reason;
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Order with {order.Id} id status changed. Status: {order.Status}");
            }
            else
            {
                _logger.LogError($"Order with {command.OrderId} id not found.");
            }
        }
    }
}
