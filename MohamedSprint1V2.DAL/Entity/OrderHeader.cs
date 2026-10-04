using MohamedSprint1V2.DAL.Entity;

namespace MohamedSprint1V2.DAL.Entity
{
    public class OrderHeader
    {
        public OrderHeader() { }
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser? ApplicationUser { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;
        public DateTime ShippingDate { get; set; }

        public decimal TotalPrice { get; set; }

        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string? PaymentMethod { get; set; }

        public string? TrakcingNumber { get; set; }
        public string? Carrier { get; set; }

        public DateTime PaymentDate { get; set; }

        //Stripe Properties
        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }

        //User Data
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string? PhoneNumber { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
