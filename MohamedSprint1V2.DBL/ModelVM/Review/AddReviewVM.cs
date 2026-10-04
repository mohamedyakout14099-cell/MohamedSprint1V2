using System.ComponentModel.DataAnnotations;

namespace MohamedSprint1V2.DLL.ModelVM.Review
{
    public class AddReviewVM
    {
        [Required]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Please select a star rating.")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5 stars.")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Please enter your review.")]
        [StringLength(1000, MinimumLength = 3, ErrorMessage = "Review must be between 3 and 1000 characters.")]
        [Display(Name = "Your Review")]
        public string Comment { get; set; } = string.Empty;
    }
}
