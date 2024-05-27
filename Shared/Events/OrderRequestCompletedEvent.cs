using Shared.Interfaces.Events;

namespace Shared.Events
{
    public class OrderRequestCompletedEvent : IOrderRequestCompletedEvent
    {
        public int OrderId { get; set; }
    }
}
