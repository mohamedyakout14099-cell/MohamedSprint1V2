using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MohamedSprint1V2.DAL.Repo.Impelementation
{
    public class ProductRepo : GenreicRepo<Product>, IProductRepo
    {
        private MohamedSprint1V2DbContext context;
        public ProductRepo(MohamedSprint1V2DbContext context) : base(context) 
        {
             this.context = context;
        }
    }
}
