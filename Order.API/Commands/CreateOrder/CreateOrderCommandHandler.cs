using Mapster;
using Order.API.Constants;
using Order.API.Contexts;
using Shared.Constants;
using Shared.Events;
using Shared.Interfaces.Commands;
using Shared.Interfaces.Services;
using Shared.Messages;

namespace Order.API.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, CreateOrderCommandResult>
    {
        private readonly AppDbContext _dbContext;
        private readonly IBusService _busService;

        public CreateOrderCommandHandler(AppDbContext dbContext, IBusService busService)
        {
            _dbContext = dbContext;
            _busService = busService;
        }

        public async Task<CreateOrderCommandResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var order = command.Adapt<Order.API.Models.Order>();
            order.Status = OrderStatus.Suspend;

            await _dbContext.Orders.AddAsync(order, cancellationToken);

            await _dbContext.SaveChangesAsync();

            var newOrderCreatedEvent = new OrderCreatedRequestEvent
            {
                OrderId = order.Id,
                BuyerId = order.BuyerId,
                OrderItems = order.Items.Adapt<List<OrderItemMessage>>(),
                Payment = command.Payment.Adapt<PaymentMessage>()
            };
            newOrderCreatedEvent.Payment.TotalPrice = newOrderCreatedEvent.OrderItems.Sum(x => x.Price * x.Count);

            await _busService.Send(RabbitMqQueues.OrderSaga, newOrderCreatedEvent);

            return new CreateOrderCommandResult
            {
                OrderId = order.Id
            };
        }
    }
}
