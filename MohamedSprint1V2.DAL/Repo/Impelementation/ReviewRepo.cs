using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MohamedSprint1V2.DAL.Repo.Impelementation
{
    public class ReviewRepo : GenreicRepo<Review>, IReviewRepo
    {
        private readonly MohamedSprint1V2DbContext _context;

        public ReviewRepo(MohamedSprint1V2DbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Review>> GetReviewsByProductIdAsync(int productId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> HasUserReviewedProductAsync(int productId, string userId)
        {
            return await _context.Reviews
                .AnyAsync(r => r.ProductId == productId && r.UserId == userId);
        }

        public async Task<double> GetAverageRatingAsync(int productId)
        {
            var ratings = await _context.Reviews
                .Where(r => r.ProductId == productId)
                .Select(r => r.Rating)
                .ToListAsync();

            if (!ratings.Any())
                return 0.0;

            return ratings.Average();
        }

        public async Task<int> GetReviewCountAsync(int productId)
        {
            return await _context.Reviews
                .CountAsync(r => r.ProductId == productId);
        }
    }
}
