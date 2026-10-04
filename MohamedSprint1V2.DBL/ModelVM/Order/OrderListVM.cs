using System;

namespace MohamedSprint1V2.DLL.ModelVM.Order
{
    public class OrderListVM
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string? CustomerEmail { get; set; }
        public string? PhoneNumber { get; set; }
        public string City { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
        public string? PaymentMethod { get; set; }
        public int ItemCount { get; set; }
    }
}
