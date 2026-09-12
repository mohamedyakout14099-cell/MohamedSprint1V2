using MohamedSprint1V2.DAL.Entity;
using MohamedSprint1V2.DLL.ModelVM.ResponseResult;
using System.Collections.Generic;

namespace MohamedSprint1V2.DLL.Service.Abstraction
{
    public interface ICartService
    {
        Cart GetCart();
        Cart? GetCartWithItems();
        Response<bool> AddToCart(int productId, int quantity);
        Response<bool> RemoveFromCart(int cartItemId);
        Response<bool> ClearCart();
    }
}
