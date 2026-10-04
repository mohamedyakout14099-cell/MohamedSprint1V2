using MohamedSprint1V2.DAL.Entity;
using System.Collections.Generic;

namespace MohamedSprint1V2.DAL.Repo.Abstraction
{
    public interface IOrderHeaderRepo : IGenreicRepo<OrderHeader>
    {
        OrderHeader? GetOrderWithDetails(int orderId);
        List<OrderHeader> GetOrdersByUserId(string userId);
        List<OrderHeader> GetAllOrdersWithUser(string? status = null);
        void UpdateStatus(int orderId, string orderStatus, string? paymentStatus = null);
        void UpdateShippingDetails(int orderId, string carrier, string trackingNumber);
        void UpdateStripePaymentId(int orderId, string sessionId, string paymentIntentId);
    }
}
