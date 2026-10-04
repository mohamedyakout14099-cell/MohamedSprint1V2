using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MohamedSprint1V2.DLL.ModelVM.Order;
using MohamedSprint1V2.DLL.Service.Abstraction;
using System.Linq;

namespace MohamedSprint1V2.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Manager,Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: /Admin/Order
        [HttpGet]
        public IActionResult Index(string? status, string? search)
        {
            var response = _orderService.GetAllOrdersForAdmin(status);
            var list = response.result ?? new System.Collections.Generic.List<OrderListVM>();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                list = list.Where(o =>
                    o.OrderId.ToString().Contains(term) ||
                    (o.CustomerName != null && o.CustomerName.ToLower().Contains(term)) ||
                    (o.CustomerEmail != null && o.CustomerEmail.ToLower().Contains(term)) ||
                    (o.City != null && o.City.ToLower().Contains(term))
                ).ToList();
                ViewBag.SearchTerm = search;
            }

            ViewBag.ActiveStatus = status ?? "All";
            return View(list);
        }

        // GET: /Admin/Order/Details/{id}
        [HttpGet]
        public IActionResult Details(int id)
        {
            var response = _orderService.GetOrderDetails(id);
            if (!response.Successornot || response.result == null)
            {
                TempData["ErrorMessage"] = response.Message ?? "Order not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(response.result);
        }

        // POST: /Admin/Order/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int orderId, string orderStatus, string? carrier, string? trackingNumber)
        {
            var response = _orderService.UpdateOrderStatus(orderId, orderStatus, carrier, trackingNumber);
            if (response.Successornot)
            {
                TempData["SuccessMessage"] = response.Message;
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
            }

            return RedirectToAction(nameof(Details), new { id = orderId });
        }
    }
}
