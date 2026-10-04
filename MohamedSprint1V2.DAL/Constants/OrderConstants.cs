namespace MohamedSprint1V2.DAL.Constants
{
    public static class OrderConstants
    {
        // Order Status
        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusInProcess = "Processing";
        public const string StatusShipped = "Shipped";
        public const string StatusDelivered = "Delivered";
        public const string StatusCancelled = "Cancelled";

        // Payment Status
        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusCashOnDelivery = "CashOnDelivery";
        public const string PaymentStatusRefunded = "Refunded";

        // Payment Methods
        public const string MethodCashOnDelivery = "Cash On Delivery";
        public const string MethodStripe = "Credit / Debit Card (Stripe)";
    }
}
