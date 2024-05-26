using MassTransit;

namespace Shared.Interfaces.Events
{
    public interface IStockNotReservedEvent : CorrelatedBy<Guid>
    {
        string Reason { get; set; }
    }
}
