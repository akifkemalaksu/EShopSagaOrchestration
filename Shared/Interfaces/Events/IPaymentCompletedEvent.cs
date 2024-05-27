using MassTransit;

namespace Shared.Interfaces.Events
{
    public interface IPaymentCompletedEvent : CorrelatedBy<Guid>
    {

    }
}
