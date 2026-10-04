using MohamedSprint1V2.DLL.ModelVM.Order;
using MohamedSprint1V2.DLL.ModelVM.ResponseResult;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MohamedSprint1V2.DLL.Service.Abstraction
{
    public interface IOrderService
    {
        Task<Response<CheckoutVM>> GetCheckoutDataAsync(string userId);
        Task<Response<int>> ProcessOrderAsync(CheckoutVM model, string userId);
        Response<OrderDetailsVM> GetOrderDetails(int orderId, string? userId = null);
        Response<List<OrderListVM>> GetUserOrders(string userId);
        Response<List<OrderListVM>> GetAllOrdersForAdmin(string? status = null);
        Response<bool> UpdateOrderStatus(int orderId, string orderStatus, string? carrier = null, string? trackingNumber = null);
    }
}
