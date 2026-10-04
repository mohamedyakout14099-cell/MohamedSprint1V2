using MohamedSprint1V2.DLL.ModelVM.ResponseResult;
using MohamedSprint1V2.DLL.ModelVM.Review;
using System.Threading.Tasks;

namespace MohamedSprint1V2.DLL.Service.Abstraction
{
    public interface IReviewService
    {
        Task<Response<bool>> AddReviewAsync(AddReviewVM model, string userId);
        Task<Response<ProductReviewsSummaryVM>> GetProductReviewsAsync(int productId, string? currentUserId = null);
        Task<Response<bool>> DeleteReviewAsync(int reviewId, string currentUserId, bool isAdmin);
    }
}
