using Shared.Messages;

namespace Shared.Interfaces.Events
{
    public interface IPaymentFailedStockRollbackEvent
    {
        List<OrderItemMessage> OrderItems { get; set; }

        public int OrderId { get; set; }
    }
}
