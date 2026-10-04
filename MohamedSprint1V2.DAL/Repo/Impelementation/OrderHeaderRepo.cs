using Microsoft.EntityFrameworkCore;
using MohamedSprint1V2.DAL.Database;
using MohamedSprint1V2.DAL.Entity;
using MohamedSprint1V2.DAL.Repo.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MohamedSprint1V2.DAL.Repo.Impelementation
{
    public class OrderHeaderRepo : GenreicRepo<OrderHeader>, IOrderHeaderRepo
    {
        private readonly MohamedSprint1V2DbContext _context;

        public OrderHeaderRepo(MohamedSprint1V2DbContext context) : base(context)
        {
            _context = context;
        }

        public OrderHeader? GetOrderWithDetails(int orderId)
        {
            return _context.OrderHeaders
                .Include(o => o.ApplicationUser)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefault(o => o.Id == orderId);
        }

        public List<OrderHeader> GetOrdersByUserId(string userId)
        {
            return _context.OrderHeaders
                .Include(o => o.OrderDetails)
                .Where(o => o.ApplicationUserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
        }

        public List<OrderHeader> GetAllOrdersWithUser(string? status = null)
        {
            var query = _context.OrderHeaders
                .Include(o => o.ApplicationUser)
                .Include(o => o.OrderDetails)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(o => o.OrderStatus == status);
            }

            return query.OrderByDescending(o => o.OrderDate).ToList();
        }

        public void UpdateStatus(int orderId, string orderStatus, string? paymentStatus = null)
        {
            var order = _context.OrderHeaders.FirstOrDefault(o => o.Id == orderId);
            if (order != null)
            {
                order.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    order.PaymentStatus = paymentStatus;
                }
            }
        }

        public void UpdateShippingDetails(int orderId, string carrier, string trackingNumber)
        {
            var order = _context.OrderHeaders.FirstOrDefault(o => o.Id == orderId);
            if (order != null)
            {
                order.Carrier = carrier;
                order.TrakcingNumber = trackingNumber;
                order.ShippingDate = DateTime.Now;
            }
        }

        public void UpdateStripePaymentId(int orderId, string sessionId, string paymentIntentId)
        {
            var order = _context.OrderHeaders.FirstOrDefault(o => o.Id == orderId);
            if (order != null)
            {
                if (!string.IsNullOrEmpty(sessionId))
                    order.SessionId = sessionId;

                if (!string.IsNullOrEmpty(paymentIntentId))
                {
                    order.PaymentIntentId = paymentIntentId;
                    order.PaymentDate = DateTime.Now;
                }
            }
        }
    }
}
