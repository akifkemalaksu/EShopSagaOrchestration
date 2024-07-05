using MassTransit;
using Shared.Messages;

namespace Shared.Interfaces.Events
{
    public interface IStockReservedEvent : CorrelatedBy<Guid>
    {
        List<OrderItemMessage> OrderItems { get; set; }
    }
}
