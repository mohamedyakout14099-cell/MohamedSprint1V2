using System;
using System.Collections.Generic;
using System.Text;

namespace MohamedSprint1V2.DAL.Repo.Abstraction
{
    public interface IUnitOfWork:IDisposable
    {
        ICategoryRepo Category { get; }
        IProductRepo Product { get; }
        ICartRepo Cart { get; }
        ICartItemRepo CartItem { get; }
        int Save();
        void Dispose();
    }
}
