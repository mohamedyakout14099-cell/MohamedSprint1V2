using Microsoft.AspNetCore.Mvc;
using MohamedSprint1V2.DLL.Service.Abstraction;

namespace MohamedSprint1V2.PL.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // GET: /Cart
        public IActionResult Index()
        {
            var cart = _cartService.GetCartWithItems();
            return View(cart);
        }

        // POST: /Cart/Add
        [HttpPost]
        public IActionResult Add(int productId, int quantity = 1)
        {
            var result = _cartService.AddToCart(productId, quantity);
            if (result.Successornot)
                TempData["SuccessMessage"] = "Product added to cart!";
            else
                TempData["ErrorMessage"] = result.Message;

            return RedirectToAction("Index");
        }

        // POST: /Cart/Remove
        [HttpPost]
        public IActionResult Remove(int cartItemId)
        {
            var result = _cartService.RemoveFromCart(cartItemId);
            if (!result.Successornot)
                TempData["ErrorMessage"] = result.Message;

            return RedirectToAction("Index");
        }

        // POST: /Cart/Clear
        [HttpPost]
        public IActionResult Clear()
        {
            _cartService.ClearCart();
            TempData["SuccessMessage"] = "Cart cleared.";
            return RedirectToAction("Index");
        }
    }
}
