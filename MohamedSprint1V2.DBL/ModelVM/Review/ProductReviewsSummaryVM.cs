using System.Collections.Generic;

namespace MohamedSprint1V2.DLL.ModelVM.Review
{
    public class ProductReviewsSummaryVM
    {
        public int ProductId { get; set; }
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public int Rating5Count { get; set; }
        public int Rating4Count { get; set; }
        public int Rating3Count { get; set; }
        public int Rating2Count { get; set; }
        public int Rating1Count { get; set; }
        public bool UserHasReviewed { get; set; }
        public List<ReviewItemVM> Reviews { get; set; } = new List<ReviewItemVM>();

        public int GetRatingPercentage(int starCount)
        {
            if (TotalReviews == 0) return 0;
            return (int)((starCount / (double)TotalReviews) * 100);
        }
    }
}
