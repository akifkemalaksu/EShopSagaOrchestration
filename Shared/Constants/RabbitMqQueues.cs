namespace Shared.Constants
{
    public class RabbitMqQueues
    {
        public const string OrderSaga = "order-saga-queue";

        public const string StockRollBackMessageQueue = "stock-rollback-queue";

        public const string StockReservedEventQueue = "stock-reserved-queue";

        public const string StockOrderCreatedEventQueue = "stock-order-created-queue";

        public const string StockPaymentFailedEventQueue = "stock-payment-failed-queue";

        public const string OrderRequestCompletedEventQueue = "order-request-completed-queue";

        public const string OrderRequestFailedEventQueue = "order-request-failed-queue";

        public const string OrderPaymentFailedEventQueue = "order-payment-failed-queue";

        public const string OrderStockNotReservedEventQueue = "order-stock-not-reserved-queue";

        public const string PaymentStockReservedRequestQueue = "payment-stock-reserved-request-queue";
    }
}
