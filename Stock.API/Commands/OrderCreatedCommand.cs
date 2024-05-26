using Microsoft.EntityFrameworkCore;
using Shared.Events;
using Shared.Interfaces.Commands;
using Shared.Interfaces.Events;
using Shared.Interfaces.Services;
using Stock.API.Contexts;

namespace Stock.API.Commands
{
    public class OrderCreatedEventHandler : ICommandHandler<IOrderCreatedEvent>
    {
        private readonly AppDbContext _dbContext;
        private readonly IBusService _busService;
        private readonly ILogger<OrderCreatedEventHandler> _logger;

        public OrderCreatedEventHandler(AppDbContext dbContext, IBusService busService, ILogger<OrderCreatedEventHandler> logger)
        {
            _dbContext = dbContext;
            _busService = busService;
            _logger = logger;
        }

        public async Task Handle(IOrderCreatedEvent command, CancellationToken cancellationToken)
        {
            var stockResult = new List<bool>();

            foreach (var item in command.OrderItems)
            {
                var isStockExist = _dbContext.Stocks.Any(x => x.ProductId == item.ProductId && x.Count > item.Count);
                stockResult.Add(isStockExist);
            }

            if (stockResult.All(x => x.Equals(true)))
            {
                foreach (var item in command.OrderItems)
                {
                    var stock = await _dbContext.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);

                    if (stock != null)
                        stock.Count -= item.Count;

                    await _dbContext.SaveChangesAsync();
                }

                _logger.LogInformation("Stock was reserved for CorrelationId: {CorrelationId}", command.CorrelationId);

                var stockReservedEvent = new StockReservedEvent(command.CorrelationId)
                {
                    OrderItems = command.OrderItems,
                };

                await _busService.Publish(stockReservedEvent);
            }
            else
            {
                _logger.LogInformation("There is not enough stock. CorrelationId: {CorrelationId}", command.CorrelationId);

                var stockNotReservedEvent = new StockNotReservedEvent(command.CorrelationId)
                {
                    Reason = "There is not enough stock."
                };

                await _busService.Publish(stockNotReservedEvent);
            }
        }
    }
}
