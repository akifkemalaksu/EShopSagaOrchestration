﻿using MassTransit;
using Shared.Messages;

namespace Shared.Interfaces.Events
{
    public interface IStockReservedRequestPayment : CorrelatedBy<Guid>
    {
        PaymentMessage Payment { get; set; }

        public List<OrderItemMessage> OrderItems { get; set; }

        public string BuyerId { get; set; }
    }
}
