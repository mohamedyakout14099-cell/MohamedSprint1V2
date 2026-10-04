using System;
using System.Collections.Generic;

namespace MohamedSprint1V2.DLL.ModelVM.Order
{
    public class OrderDetailsVM
    {
        public int OrderId { get; set; }
        public string ApplicationUserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public DateTime? ShippingDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
        public string? PaymentMethod { get; set; }
        public string? Carrier { get; set; }
        public string? TrackingNumber { get; set; }
        public List<OrderItemVM> Items { get; set; } = new List<OrderItemVM>();
    }
}
