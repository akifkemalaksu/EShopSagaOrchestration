﻿namespace Shared.Messages
{
    public class OrderItemMessage
    {
        public int ProductId { get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }
    }
}
