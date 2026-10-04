using MohamedSprint1V2.DAL.Entity;
using System.Collections.Generic;

namespace MohamedSprint1V2.DAL.Repo.Abstraction
{
    public interface IOrderDetailRepo : IGenreicRepo<OrderDetail>
    {
        List<OrderDetail> GetDetailsByOrderId(int orderId);
    }
}
