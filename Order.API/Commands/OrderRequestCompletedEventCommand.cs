using Order.API.Constants;
using Order.API.Contexts;
using Shared.Interfaces.Commands;
using Shared.Interfaces.Events;

namespace Order.API.Commands
{
    public class OrderRequestCompletedEventCommand : ICommandHandler<IOrderRequestCompletedEvent>
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<OrderRequestCompletedEventCommand> _logger;

        public OrderRequestCompletedEventCommand(AppDbContext appDbContext, ILogger<OrderRequestCompletedEventCommand> logger)
        {
            _dbContext = appDbContext;
            _logger = logger;
        }

        public async Task Handle(IOrderRequestCompletedEvent command, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders.FindAsync(command.OrderId);

            if (order != null)
            {
                order.Status = OrderStatus.Complete;
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
