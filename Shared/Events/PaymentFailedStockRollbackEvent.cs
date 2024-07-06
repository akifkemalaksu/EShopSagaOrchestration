using Shared.Interfaces.Events;
using Shared.Messages;

namespace Shared.Events
{
    public class PaymentFailedStockRollbackEvent : IPaymentFailedStockRollbackEvent
    {
        public List<OrderItemMessage> OrderItems { get; set; }

        public int OrderId { get; set; }
    }
}
