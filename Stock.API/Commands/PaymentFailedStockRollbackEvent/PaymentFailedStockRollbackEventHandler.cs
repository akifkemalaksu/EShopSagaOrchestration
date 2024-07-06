using Microsoft.EntityFrameworkCore;
using Shared.Interfaces.Commands;
using Shared.Interfaces.Events;
using Stock.API.Contexts;

namespace Stock.API.Commands.PaymentFailedStockRollbackEvent
{
    public class PaymentFailedStockRollbackEventHandler : ICommandHandler<IPaymentFailedStockRollbackEvent>
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<PaymentFailedStockRollbackEventHandler> _logger;

        public PaymentFailedStockRollbackEventHandler(AppDbContext dbContext, ILogger<PaymentFailedStockRollbackEventHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(IPaymentFailedStockRollbackEvent command, CancellationToken cancellationToken)
        {
            foreach (var item in command.OrderItems)
            {
                var stock = await _dbContext.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId, cancellationToken);
                if (stock != null)
                {
                    stock.Count += item.Count;
                }
            }

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Stock was released for Order Id: {command.OrderId}");
        }
    }
}
