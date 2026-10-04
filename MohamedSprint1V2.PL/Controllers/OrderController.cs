using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MohamedSprint1V2.DAL.Entity;
using MohamedSprint1V2.DLL.ModelVM.Order;
using MohamedSprint1V2.DLL.Service.Abstraction;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MohamedSprint1V2.PL.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        public OrderController(
            IOrderService orderService,
            UserManager<ApplicationUser> userManager,
            IEmailService emailService)
        {
            _orderService = orderService;
            _userManager = userManager;
            _emailService = emailService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        // GET: /Order/Checkout
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var userId = GetUserId();
            var response = await _orderService.GetCheckoutDataAsync(userId);

            if (!response.Successornot || response.result == null)
            {
                TempData["ErrorMessage"] = response.Message ?? "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            return View(response.result);
        }

        // POST: /Order/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutVM model)
        {
            var userId = GetUserId();

            if (!ModelState.IsValid)
            {
                var checkoutData = await _orderService.GetCheckoutDataAsync(userId);
                if (checkoutData.result != null)
                {
                    model.Items = checkoutData.result.Items;
                    model.SubTotal = checkoutData.result.SubTotal;
                    model.ShippingCost = checkoutData.result.ShippingCost;
                }
                return View(model);
            }

            var response = await _orderService.ProcessOrderAsync(model, userId);
            if (!response.Successornot)
            {
                TempData["ErrorMessage"] = response.Message;
                var checkoutData = await _orderService.GetCheckoutDataAsync(userId);
                if (checkoutData.result != null)
                {
                    model.Items = checkoutData.result.Items;
                    model.SubTotal = checkoutData.result.SubTotal;
                }
                return View(model);
            }

            // ── Send Order Confirmation Email (fire-and-forget, won't block the user) ──
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null && !string.IsNullOrEmpty(user.Email))
            {
                _ = _emailService.SendOrderConfirmationEmailAsync(
                    toEmail: user.Email,
                    toName: user.Name ?? model.Name,
                    orderId: response.result,
                    total: model.OrderTotal,
                    city: model.City,
                    address: model.Address
                );
            }

            return RedirectToAction(nameof(OrderSuccess), new { id = response.result });
        }

        // GET: /Order/OrderSuccess/{id}
        [HttpGet]
        public IActionResult OrderSuccess(int id)
        {
            var userId = GetUserId();
            var response = _orderService.GetOrderDetails(id, userId);

            if (!response.Successornot || response.result == null)
            {
                TempData["ErrorMessage"] = "Order not found.";
                return RedirectToAction("Index", "Home");
            }

            return View(response.result);
        }

        // GET: /Order/MyOrders
        [HttpGet]
        public IActionResult MyOrders()
        {
            var userId = GetUserId();
            var response = _orderService.GetUserOrders(userId);
            return View(response.result ?? new System.Collections.Generic.List<OrderListVM>());
        }

        // GET: /Order/Details/{id}
        [HttpGet]
        public IActionResult Details(int id)
        {
            var userId = GetUserId();
            var response = _orderService.GetOrderDetails(id, userId);

            if (!response.Successornot || response.result == null)
            {
                TempData["ErrorMessage"] = "Order not found or unauthorized.";
                return RedirectToAction(nameof(MyOrders));
            }

            return View(response.result);
        }
    }
}
