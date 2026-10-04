using Microsoft.AspNetCore.Identity;
using MohamedSprint1V2.DAL.Constants;
using MohamedSprint1V2.DAL.Entity;
using MohamedSprint1V2.DAL.Repo.Abstraction;
using MohamedSprint1V2.DLL.ModelVM.Order;
using MohamedSprint1V2.DLL.ModelVM.ResponseResult;
using MohamedSprint1V2.DLL.Service.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MohamedSprint1V2.DLL.Service.Impelementation
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderService(IUnitOfWork unitOfWork, ICartService cartService, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _cartService = cartService;
            _userManager = userManager;
        }

        public async Task<Response<CheckoutVM>> GetCheckoutDataAsync(string userId)
        {
            try
            {
                var cart = _cartService.GetCartWithItems();
                if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
                {
                    return new Response<CheckoutVM>(null, "Your cart is empty.", false);
                }

                var user = await _userManager.FindByIdAsync(userId);

                var items = cart.CartItems.Select(ci => new OrderItemVM
                {
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.Name ?? "Product",
                    ProductImg = ci.Product?.Img,
                    Price = ci.Product?.Price ?? 0,
                    Quantity = ci.Quantity
                }).ToList();

                var subTotal = items.Sum(i => i.SubTotal);

                var checkoutVm = new CheckoutVM
                {
                    Name = user?.Name ?? string.Empty,
                    PhoneNumber = user?.PhoneNumber ?? string.Empty,
                    Address = user?.Address ?? string.Empty,
                    City = user?.City ?? string.Empty,
                    PaymentMethod = OrderConstants.MethodCashOnDelivery,
                    Items = items,
                    SubTotal = subTotal,
                    ShippingCost = 0.00m
                };

                return new Response<CheckoutVM>(checkoutVm, "Checkout data retrieved successfully.", true);
            }
            catch (Exception ex)
            {
                return new Response<CheckoutVM>(null, ex.Message, false);
            }
        }

        public async Task<Response<int>> ProcessOrderAsync(CheckoutVM model, string userId)
        {
            try
            {
                var cart = _cartService.GetCartWithItems();
                if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
                {
                    return new Response<int>(0, "Your cart is empty. Cannot process order.", false);
                }

                decimal total = cart.CartItems.Sum(ci => (ci.Product?.Price ?? 0) * ci.Quantity);

                var orderHeader = new OrderHeader
                {
                    ApplicationUserId = userId,
                    OrderDate = DateTime.Now,
                    TotalPrice = total,
                    OrderStatus = OrderConstants.StatusPending,
                    PaymentStatus = OrderConstants.PaymentStatusCashOnDelivery,
                    PaymentMethod = model.PaymentMethod ?? OrderConstants.MethodCashOnDelivery,
                    Name = model.Name,
                    Address = model.Address,
                    City = model.City,
                    PhoneNumber = model.PhoneNumber
                };

                _unitOfWork.OrderHeader.Add(orderHeader);
                _unitOfWork.Save();

                foreach (var ci in cart.CartItems)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderHeaderId = orderHeader.Id,
                        ProductId = ci.ProductId,
                        Price = ci.Product?.Price ?? 0,
                        Count = ci.Quantity
                    };
                    _unitOfWork.OrderDetail.Add(orderDetail);
                }

                _unitOfWork.Save();

                // Clear customer's cart after successful order creation
                _cartService.ClearCart();

                return new Response<int>(orderHeader.Id, "Order placed successfully!", true);
            }
            catch (Exception ex)
            {
                return new Response<int>(0, ex.Message, false);
            }
        }

        public Response<OrderDetailsVM> GetOrderDetails(int orderId, string? userId = null)
        {
            try
            {
                var order = _unitOfWork.OrderHeader.GetOrderWithDetails(orderId);
                if (order == null)
                {
                    return new Response<OrderDetailsVM>(null, "Order not found.", false);
                }

                // If a userId was passed, ensure the order belongs to this user
                if (!string.IsNullOrEmpty(userId) && order.ApplicationUserId != userId)
                {
                    return new Response<OrderDetailsVM>(null, "You are not authorized to view this order.", false);
                }

                var vm = new OrderDetailsVM
                {
                    OrderId = order.Id,
                    ApplicationUserId = order.ApplicationUserId,
                    Name = order.Name,
                    Email = order.ApplicationUser?.Email,
                    PhoneNumber = order.PhoneNumber,
                    Address = order.Address,
                    City = order.City,
                    OrderDate = order.OrderDate,
                    ShippingDate = order.ShippingDate != default ? order.ShippingDate : (DateTime?)null,
                    TotalPrice = order.TotalPrice,
                    OrderStatus = order.OrderStatus ?? OrderConstants.StatusPending,
                    PaymentStatus = order.PaymentStatus ?? OrderConstants.PaymentStatusPending,
                    PaymentMethod = order.PaymentMethod ?? OrderConstants.MethodCashOnDelivery,
                    Carrier = order.Carrier,
                    TrackingNumber = order.TrakcingNumber,
                    Items = order.OrderDetails.Select(od => new OrderItemVM
                    {
                        ProductId = od.ProductId,
                        ProductName = od.Product?.Name ?? "Product",
                        ProductImg = od.Product?.Img,
                        Price = od.Price,
                        Quantity = od.Count
                    }).ToList()
                };

                return new Response<OrderDetailsVM>(vm, "Order details retrieved.", true);
            }
            catch (Exception ex)
            {
                return new Response<OrderDetailsVM>(null, ex.Message, false);
            }
        }

        public Response<List<OrderListVM>> GetUserOrders(string userId)
        {
            try
            {
                var orders = _unitOfWork.OrderHeader.GetOrdersByUserId(userId);
                var list = orders.Select(o => new OrderListVM
                {
                    OrderId = o.Id,
                    CustomerName = o.Name,
                    PhoneNumber = o.PhoneNumber,
                    City = o.City,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    OrderStatus = o.OrderStatus ?? OrderConstants.StatusPending,
                    PaymentStatus = o.PaymentStatus ?? OrderConstants.PaymentStatusPending,
                    PaymentMethod = o.PaymentMethod ?? OrderConstants.MethodCashOnDelivery,
                    ItemCount = o.OrderDetails?.Sum(od => od.Count) ?? 0
                }).ToList();

                return new Response<List<OrderListVM>>(list, "Orders retrieved.", true);
            }
            catch (Exception ex)
            {
                return new Response<List<OrderListVM>>(null, ex.Message, false);
            }
        }

        public Response<List<OrderListVM>> GetAllOrdersForAdmin(string? status = null)
        {
            try
            {
                var orders = _unitOfWork.OrderHeader.GetAllOrdersWithUser(status);
                var list = orders.Select(o => new OrderListVM
                {
                    OrderId = o.Id,
                    CustomerName = o.Name,
                    CustomerEmail = o.ApplicationUser?.Email,
                    PhoneNumber = o.PhoneNumber,
                    City = o.City,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    OrderStatus = o.OrderStatus ?? OrderConstants.StatusPending,
                    PaymentStatus = o.PaymentStatus ?? OrderConstants.PaymentStatusPending,
                    PaymentMethod = o.PaymentMethod ?? OrderConstants.MethodCashOnDelivery,
                    ItemCount = o.OrderDetails?.Sum(od => od.Count) ?? 0
                }).ToList();

                return new Response<List<OrderListVM>>(list, "All orders retrieved.", true);
            }
            catch (Exception ex)
            {
                return new Response<List<OrderListVM>>(null, ex.Message, false);
            }
        }

        public Response<bool> UpdateOrderStatus(int orderId, string orderStatus, string? carrier = null, string? trackingNumber = null)
        {
            try
            {
                _unitOfWork.OrderHeader.UpdateStatus(orderId, orderStatus);

                if (!string.IsNullOrWhiteSpace(carrier) || !string.IsNullOrWhiteSpace(trackingNumber))
                {
                    _unitOfWork.OrderHeader.UpdateShippingDetails(orderId, carrier ?? string.Empty, trackingNumber ?? string.Empty);
                }

                _unitOfWork.Save();
                return new Response<bool>(true, $"Order status updated to {orderStatus}.", true);
            }
            catch (Exception ex)
            {
                return new Response<bool>(false, ex.Message, false);
            }
        }
    }
}
