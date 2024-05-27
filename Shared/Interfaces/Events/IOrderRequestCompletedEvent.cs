namespace Shared.Interfaces.Events
{
    public interface IOrderRequestCompletedEvent
    {
        public int OrderId { get; set; }
    }
}
