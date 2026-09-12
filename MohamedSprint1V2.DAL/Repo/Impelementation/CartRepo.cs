using Microsoft.EntityFrameworkCore;
using MohamedSprint1V2.DAL.Database;
using MohamedSprint1V2.DAL.Entity;
using MohamedSprint1V2.DAL.Repo.Abstraction;

namespace MohamedSprint1V2.DAL.Repo.Impelementation
{
    public class CartRepo : GenreicRepo<Cart>, ICartRepo
    {
        private readonly MohamedSprint1V2DbContext _context;

        public CartRepo(MohamedSprint1V2DbContext context) : base(context)
        {
            _context = context;
        }

        public Cart? GetCartWithItems(string userId, bool isUser)
        {
            if (isUser)
                return _context.Carts
                    .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                    .FirstOrDefault(c => c.ApplicationUserId == userId);

            return _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefault(c => c.SessionId == userId);
        }
    }
}
