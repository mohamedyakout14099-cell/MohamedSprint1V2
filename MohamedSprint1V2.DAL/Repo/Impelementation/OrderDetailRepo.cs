using Microsoft.EntityFrameworkCore;
using MohamedSprint1V2.DAL.Database;
using MohamedSprint1V2.DAL.Entity;
using MohamedSprint1V2.DAL.Repo.Abstraction;
using System.Collections.Generic;
using System.Linq;

namespace MohamedSprint1V2.DAL.Repo.Impelementation
{
    public class OrderDetailRepo : GenreicRepo<OrderDetail>, IOrderDetailRepo
    {
        private readonly MohamedSprint1V2DbContext _context;

        public OrderDetailRepo(MohamedSprint1V2DbContext context) : base(context)
        {
            _context = context;
        }

        public List<OrderDetail> GetDetailsByOrderId(int orderId)
        {
            return _context.OrderDetails
                .Include(od => od.Product)
                .Where(od => od.OrderHeaderId == orderId)
                .ToList();
        }
    }
}
