using MohamedSprint1V2.DAL.Database;
using MohamedSprint1V2.DAL.Entity;
using MohamedSprint1V2.DAL.Repo.Abstraction;

namespace MohamedSprint1V2.DAL.Repo.Impelementation
{
    public class CartItemRepo : GenreicRepo<CartItem>, ICartItemRepo
    {
        public CartItemRepo(MohamedSprint1V2DbContext context) : base(context)
        {
        }
    }
}
