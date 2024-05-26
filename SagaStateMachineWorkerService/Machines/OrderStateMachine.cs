using Mapster;
using MassTransit;
using SagaStateMachineWorkerService.Models;
using Shared.Interfaces.Events;

namespace SagaStateMachineWorkerService.Machines
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
    {
        public Event<IOrderCreatedRequestEvent> OrderCreatedRequestEvent { get; set; }

        public State OrderCreated { get; private set; }

        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(
                () => OrderCreatedRequestEvent,
                property =>
                    property
                        .CorrelateBy<int>(x => x.OrderId, z => z.Message.OrderId)
                        .SelectId(context => Guid.NewGuid())
                );

            Initially(
                When(OrderCreatedRequestEvent)
                .Then(context =>
                {
                    context.Data.Adapt(context.Instance);
                })
                .Then(context => Console.WriteLine($"{nameof(OrderCreatedRequestEvent)} before: {context.Instance}"))
                .TransitionTo(OrderCreated)
                .Then(context => Console.WriteLine($"{nameof(OrderCreatedRequestEvent)} after: {context.Instance}"))
                );
        }
    }
}
