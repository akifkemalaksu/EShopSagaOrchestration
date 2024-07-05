using Mapster;
using MassTransit;
using SagaStateMachineWorkerService.Models;
using Shared.Constants;
using Shared.Events;
using Shared.Interfaces.Events;
using Shared.Messages;

namespace SagaStateMachineWorkerService.Machines
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
    {
        public Event<IOrderCreatedRequestEvent> OrderCreatedRequestEvent { get; set; }

        public Event<IStockReservedEvent> StockReservedEvent { get; set; }

        public Event<IStockNotReservedEvent> StockNotReservedEvent { get; set; }

        public Event<IPaymentCompletedEvent> PaymentCompletedEvent { get; set; }

        public State OrderCreated { get; private set; }

        public State StockReserved { get; private set; }

        public State StockNotReserved { get; private set; }

        public State PaymentCompleted { get; private set; }

        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(
                () => OrderCreatedRequestEvent,
                x => x.CorrelateBy<int>(x => x.OrderId, z => z.Message.OrderId)
                        .SelectId(context => Guid.NewGuid()));

            Event(
                () => StockReservedEvent,
                x => x.CorrelateById(y => y.Message.CorrelationId));

            Event(
                () => StockNotReservedEvent,
                x => x.CorrelateById(y => y.Message.CorrelationId));

            Event(
                () => PaymentCompletedEvent,
                x => x.CorrelateById(y => y.Message.CorrelationId));

            Initially(
                When(OrderCreatedRequestEvent)
                .Then(context =>
                {
                    context.Message.Adapt(context.Saga);
                })
                .Then(context => Console.WriteLine($"{nameof(OrderCreatedRequestEvent)} before: {context.Saga}"))
                .PublishAsync(
                    async context =>
                    await context.Init<IOrderCreatedEvent>(
                        new OrderCreatedEvent(context.Saga.CorrelationId)
                        {
                            OrderItems = context.Message.OrderItems
                        }
                    )
                )
                .TransitionTo(OrderCreated)
                .Then(context => Console.WriteLine($"{nameof(OrderCreatedRequestEvent)} after: {context.Saga}"))
            );

            During(OrderCreated,

                When(StockReservedEvent)
                .TransitionTo(StockReserved)
                .SendAsync(
                    new Uri($"queue:{RabbitMqQueues.PaymentStockReservedRequestQueue}"),
                    async context =>
                    await context.Init<IStockReservedRequestPaymentEvent>(
                        new StockReservedRequestPaymentEvent(context.Saga.CorrelationId)
                        {
                            OrderItems = context.Message.OrderItems,
                            Payment = context.Saga.Payment.Adapt<PaymentMessage>(),
                            BuyerId = context.Saga.BuyerId
                        }
                    )
                )
                .Then(context => Console.WriteLine($"{nameof(StockReservedEvent)} after: {context.Saga}")),

                When(StockNotReservedEvent)
                .TransitionTo(StockNotReserved)
                .SendAsync(
                    new Uri($"queue:{RabbitMqQueues.OrderRequestFailedEventQueue}"),
                    async context =>
                    await context.Init<IOrderRequestFailedEvent>(
                        new OrderRequestFailedEvent()
                        {
                            OrderId = context.Saga.OrderId,
                            Reason = context.Message.Reason,
                        }
                        )
                    )
                .Then(context => Console.WriteLine($"{nameof(StockReservedEvent)} after: {context.Saga}"))
            );

            During(StockReserved,

                When(PaymentCompletedEvent)
                .TransitionTo(PaymentCompleted)
                .PublishAsync(
                    async context =>
                    await context.Init<IOrderRequestCompletedEvent>(
                        new OrderRequestCompletedEvent
                        {
                            OrderId = context.Saga.OrderId
                        }
                    )
                )
                .Then(context => Console.WriteLine($"{nameof(PaymentCompletedEvent)} after: {context.Saga}"))
                .Finalize()

            );
        }
    }
}
