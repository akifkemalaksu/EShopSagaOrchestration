using MassTransit;
using Shared.Messages;

namespace Shared.Interfaces.Events
{
    public interface IOrderCreatedEvent : CorrelatedBy<Guid>
    {
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
