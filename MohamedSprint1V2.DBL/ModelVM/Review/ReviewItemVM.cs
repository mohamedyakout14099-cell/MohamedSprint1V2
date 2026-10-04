using System;

namespace MohamedSprint1V2.DLL.ModelVM.Review
{
    public class ReviewItemVM
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? UserEmail { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsCurrentCustomerReview { get; set; }
    }
}
