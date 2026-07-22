using MohamedSprint1V2.DAL.Entity;

namespace MohamedSprint1V2.DAL.Entity
{
    public class OrderHeader
    {
        protected OrderHeader() { }
        public int Id { get; private set; }

        public string ApplicationUserId { get; private set; }

        [ValidateNever]
        public ApplicationUser ApplicationUser { get; private set; }

        public DateTime OrderDate { get; private    set; }
        public DateTime ShippingDate { get; private set; }

        public decimal TotalPrice { get; private set; }

        public string? OrderStatus { get; private set; }
        public string? PaymentStatus { get; private set; }

        public string? TrakcingNumber { get; private set; }
        public string? Carrier { get;private set; }

        public DateTime PaymentDate { get; private set; }

        //Stripe Properties

        public string? SessionId { get; private set; }
        public string? PaymentIntentId { get; private set; }

        //User Data
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string City { get; private set; }
        public string? PhoneNumber { get; private set; }

    }
}
