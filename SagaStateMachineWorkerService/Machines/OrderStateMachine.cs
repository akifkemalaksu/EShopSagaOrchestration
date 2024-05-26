using Mapster;
using MassTransit;
using SagaStateMachineWorkerService.Models;
using Shared.Events;
using Shared.Interfaces.Events;
using Shared.Messages;
using Shared.Settings;

namespace SagaStateMachineWorkerService.Machines
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
    {
        public Event<IOrderCreatedRequestEvent> OrderCreatedRequestEvent { get; set; }

        public Event<IStockReservedEvent> StockReservedEvent { get; set; }

        public State OrderCreated { get; private set; }

        public State StockReserved { get; private set; }

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
                    context.Message.Adapt(context.Saga);
                })
                .Then(context => Console.WriteLine($"{nameof(OrderCreatedRequestEvent)} before: {context.Instance}"))
                .PublishAsync(context =>
                    context.Init<IOrderCreatedEvent>(
                        new OrderCreatedEvent(context.Saga.CorrelationId)
                        {
                            OrderItems = context.Message.OrderItems
                        }
                    )
                )
                .TransitionTo(OrderCreated)
                .Then(context => Console.WriteLine($"{nameof(OrderCreatedRequestEvent)} after: {context.Instance}"))
            );

            During(OrderCreated,
                When(StockReservedEvent)
                .TransitionTo(StockReserved)
                .SendAsync(
                    new Uri($"queue:{RabbitMqQueues.PaymentStockReservedRequestQueue}"),
                    context => context.Init<IStockReservedRequestPayment>(new StockReservedRequestPayment(context.Saga.CorrelationId)
                    {
                        OrderItems = context.Message.OrderItems,
                        Payment = context.Saga.Payment.Adapt<PaymentMessage>()
                    })
                )
                .Then(context => Console.WriteLine($"{nameof(StockReservedEvent)} after: {context.Saga}"))
            );

            During(StockReserved);
        }
    }
}
