using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MohamedSprint1V2.DAL.Entity;
using MohamedSprint1V2.DAL.Repo.Abstraction;
using MohamedSprint1V2.DLL.ModelVM.ResponseResult;
using MohamedSprint1V2.DLL.Service.Abstraction;
using System;
using System.Linq;

namespace MohamedSprint1V2.DLL.Service.Impelementation
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private const string SessionCartId = "SessionCartId";

        public CartService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        private string GetSessionOrUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
            {
                return _userManager.GetUserId(user);
            }

            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                var cartId = session.GetString(SessionCartId);
                if (string.IsNullOrEmpty(cartId))
                {
                    cartId = Guid.NewGuid().ToString();
                    session.SetString(SessionCartId, cartId);
                }
                return cartId;
            }

            return Guid.NewGuid().ToString();
        }

        public Cart GetCart()
        {
            var userOrSessionId = GetSessionOrUserId();
            var isUser = _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

            Cart cart;
            if (isUser)
            {
                cart = _unitOfWork.Cart.getAll(c => c.ApplicationUserId == userOrSessionId).FirstOrDefault();
            }
            else
            {
                cart = _unitOfWork.Cart.getAll(c => c.SessionId == userOrSessionId).FirstOrDefault();
            }

            if (cart == null)
            {
                cart = new Cart
                {
                    ApplicationUserId = isUser ? userOrSessionId : null,
                    SessionId = isUser ? null : userOrSessionId,
                    CreatedAt = DateTime.Now
                };
                _unitOfWork.Cart.Add(cart);
                _unitOfWork.Save();
            }

            // Load CartItems and Products via dedicated Include method.
            return cart;
        }

        public Cart? GetCartWithItems()
        {
            var userOrSessionId = GetSessionOrUserId();
            var isUser = _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
            return _unitOfWork.Cart.GetCartWithItems(userOrSessionId, isUser);
        }

        public Response<bool> AddToCart(int productId, int quantity)
        {
            try
            {
                var cart = GetCart();
                var existingItem = _unitOfWork.CartItem.getAll(ci => ci.CartId == cart.Id && ci.ProductId == productId).FirstOrDefault();

                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                    _unitOfWork.CartItem.Update(existingItem);
                }
                else
                {
                    var cartItem = new CartItem
                    {
                        CartId = cart.Id,
                        ProductId = productId,
                        Quantity = quantity
                    };
                    _unitOfWork.CartItem.Add(cartItem);
                }
                
                _unitOfWork.Save();
                return new Response<bool>(true, "Added to cart", true);
            }
            catch (Exception ex)
            {
                return new Response<bool>(false, ex.Message, false);
            }
        }

        public Response<bool> RemoveFromCart(int cartItemId)
        {
            try
            {
                var item = _unitOfWork.CartItem.GetById(cartItemId);
                if (item != null)
                {
                    _unitOfWork.CartItem.Delete(cartItemId);
                    _unitOfWork.Save();
                }
                return new Response<bool>(true, "Removed", true);
            }
            catch (Exception ex)
            {
                return new Response<bool>(false, ex.Message, false);
            }
        }

        public Response<bool> ClearCart()
        {
            try
            {
                var cart = GetCart();
                var items = _unitOfWork.CartItem.getAll(ci => ci.CartId == cart.Id);
                foreach (var item in items)
                {
                    _unitOfWork.CartItem.Delete(item.Id);
                }
                _unitOfWork.Save();
                return new Response<bool>(true, "Cleared", true);
            }
            catch (Exception ex)
            {
                return new Response<bool>(false, ex.Message, false);
            }
        }
    }
}
