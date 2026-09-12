using MohamedSprint1V2.DAL.Entity;

namespace MohamedSprint1V2.DAL.Repo.Abstraction
{
    public interface ICartRepo : IGenreicRepo<Cart>
    {
        Cart? GetCartWithItems(string userId, bool isUser);
    }
}
