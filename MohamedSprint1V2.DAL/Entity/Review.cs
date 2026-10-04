using System;

namespace MohamedSprint1V2.DAL.Entity
{
    public class Review
    {
        public Review() { }

        public Review(int productId, string userId, int rating, string comment)
        {
            ProductId = productId;
            UserId = userId;
            Rating = rating;
            Comment = comment;
            CreatedAt = DateTime.UtcNow;
            IsDeleted = false;
        }

        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        public int Rating { get; set; } // 1 - 5 stars
        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
    }
}
