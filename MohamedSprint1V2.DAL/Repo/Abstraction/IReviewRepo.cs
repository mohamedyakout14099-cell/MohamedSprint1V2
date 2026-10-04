using System.Collections.Generic;
using System.Threading.Tasks;

namespace MohamedSprint1V2.DAL.Repo.Abstraction
{
    public interface IReviewRepo : IGenreicRepo<Review>
    {
        Task<List<Review>> GetReviewsByProductIdAsync(int productId);
        Task<bool> HasUserReviewedProductAsync(int productId, string userId);
        Task<double> GetAverageRatingAsync(int productId);
        Task<int> GetReviewCountAsync(int productId);
    }
}
