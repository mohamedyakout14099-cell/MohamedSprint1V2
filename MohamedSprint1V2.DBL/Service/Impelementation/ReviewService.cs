using MohamedSprint1V2.DAL.Entity;
using MohamedSprint1V2.DAL.Repo.Abstraction;
using MohamedSprint1V2.DLL.ModelVM.ResponseResult;
using MohamedSprint1V2.DLL.ModelVM.Review;
using MohamedSprint1V2.DLL.Service.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MohamedSprint1V2.DLL.Service.Impelementation
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> AddReviewAsync(AddReviewVM model, string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return new Response<bool>(false, "User is not authenticated.", false);
                }

                if (model.Rating < 1 || model.Rating > 5)
                {
                    return new Response<bool>(false, "Rating must be between 1 and 5 stars.", false);
                }

                // Check if product exists
                var product = _unitOfWork.Product.GetById(model.ProductId);
                if (product == null)
                {
                    return new Response<bool>(false, "Product not found.", false);
                }

                // Check if user already reviewed this product
                var hasReviewed = await _unitOfWork.Review.HasUserReviewedProductAsync(model.ProductId, userId);
                if (hasReviewed)
                {
                    return new Response<bool>(false, "You have already reviewed this product.", false);
                }

                var review = new Review(model.ProductId, userId, model.Rating, model.Comment?.Trim() ?? string.Empty);
                _unitOfWork.Review.Add(review);
                var saved = _unitOfWork.Save();

                if (saved > 0)
                {
                    return new Response<bool>(true, "Thank you! Your review has been added.", true);
                }

                return new Response<bool>(false, "Failed to submit review.", false);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return new Response<bool>(false, $"Error adding review: {message}", false);
            }
        }

        public async Task<Response<ProductReviewsSummaryVM>> GetProductReviewsAsync(int productId, string? currentUserId = null)
        {
            try
            {
                var reviews = await _unitOfWork.Review.GetReviewsByProductIdAsync(productId);
                var totalReviews = reviews.Count;
                var averageRating = totalReviews > 0 ? reviews.Average(r => r.Rating) : 0.0;

                var reviewItems = reviews.Select(r => new ReviewItemVM
                {
                    Id = r.Id,
                    ProductId = r.ProductId,
                    UserId = r.UserId,
                    UserName = !string.IsNullOrEmpty(r.User?.Name) ? r.User.Name : (r.User?.UserName ?? "Anonymous Customer"),
                    UserEmail = r.User?.Email,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt,
                    IsCurrentCustomerReview = !string.IsNullOrEmpty(currentUserId) && r.UserId == currentUserId
                }).ToList();

                var summary = new ProductReviewsSummaryVM
                {
                    ProductId = productId,
                    AverageRating = Math.Round(averageRating, 1),
                    TotalReviews = totalReviews,
                    Rating5Count = reviews.Count(r => r.Rating == 5),
                    Rating4Count = reviews.Count(r => r.Rating == 4),
                    Rating3Count = reviews.Count(r => r.Rating == 3),
                    Rating2Count = reviews.Count(r => r.Rating == 2),
                    Rating1Count = reviews.Count(r => r.Rating == 1),
                    UserHasReviewed = !string.IsNullOrEmpty(currentUserId) && reviews.Any(r => r.UserId == currentUserId),
                    Reviews = reviewItems
                };

                return new Response<ProductReviewsSummaryVM>(summary, "Reviews loaded successfully.", true);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return new Response<ProductReviewsSummaryVM>(new ProductReviewsSummaryVM { ProductId = productId }, $"Error loading reviews: {message}", false);
            }
        }

        public async Task<Response<bool>> DeleteReviewAsync(int reviewId, string currentUserId, bool isAdmin)
        {
            try
            {
                var review = _unitOfWork.Review.GetById(reviewId);
                if (review == null)
                {
                    return new Response<bool>(false, "Review not found.", false);
                }

                if (!isAdmin && review.UserId != currentUserId)
                {
                    return new Response<bool>(false, "You are not authorized to delete this review.", false);
                }

                _unitOfWork.Review.SoftDelete(reviewId);
                var saved = _unitOfWork.Save();

                if (saved > 0)
                {
                    return new Response<bool>(true, "Review deleted successfully.", true);
                }

                return new Response<bool>(false, "Failed to delete review.", false);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return new Response<bool>(false, $"Error deleting review: {message}", false);
            }
        }
    }
}
