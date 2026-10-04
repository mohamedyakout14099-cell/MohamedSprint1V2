using System;
using System.Collections.Generic;

namespace MohamedSprint1V2.DAL.Entity
{
    public class Cart
    {
        public int Id { get; set; }

        public string? SessionId { get; set; } 

        public string? ApplicationUserId { get; set; }

        public ApplicationUser? ApplicationUser { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
