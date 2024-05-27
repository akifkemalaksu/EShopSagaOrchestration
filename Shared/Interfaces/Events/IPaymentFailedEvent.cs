﻿using MassTransit;

namespace Shared.Interfaces.Events
{
    public interface IPaymentFailedEvent : CorrelatedBy<Guid>
    {
        public string Reason { get; set; }

        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
