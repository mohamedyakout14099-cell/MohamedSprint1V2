 using System;
using System.Collections.Generic;
using System.Text;

namespace MohamedSprint1V2.DAL.Repo.Impelementation
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly MohamedSprint1V2DbContext context;

        public UnitOfWork(MohamedSprint1V2DbContext context)
        {
            this.context = context;
            Category = new CategoryRepo(context);
            Product= new ProductRepo(context);
            Cart = new CartRepo(context);
            CartItem = new CartItemRepo(context);
            OrderHeader = new OrderHeaderRepo(context);
            OrderDetail = new OrderDetailRepo(context);
        }
        public ICategoryRepo Category{ get;private set;  }
        public IProductRepo Product { get; private set; }
        public ICartRepo Cart { get; private set; }
        public ICartItemRepo CartItem { get; private set; }
        public IOrderHeaderRepo OrderHeader { get; private set; }
        public IOrderDetailRepo OrderDetail { get; private set; }
        public int Save()
        {
            return context.SaveChanges();
        }
        public void Dispose()
        {
            context.Dispose();
        }
    }
}
