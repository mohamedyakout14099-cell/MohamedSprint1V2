using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MohamedSprint1V2.DLL.ModelVM.Order
{
    public class CheckoutVM
    {
        [Required(ErrorMessage = "Full Name is required")]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone Number is required")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Street Address is required")]
        [Display(Name = "Street Address")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required")]
        [Display(Name = "City")]
        public string City { get; set; } = string.Empty;

        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; } = "Cash On Delivery";

        [Display(Name = "Order Notes (Optional)")]
        public string? OrderNotes { get; set; }

        public List<OrderItemVM> Items { get; set; } = new List<OrderItemVM>();

        public decimal SubTotal { get; set; }
        public decimal ShippingCost { get; set; } = 0.00m;
        public decimal OrderTotal => SubTotal + ShippingCost;
    }
}
