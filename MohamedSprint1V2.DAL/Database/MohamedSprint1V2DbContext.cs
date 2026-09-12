using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//using MohamedSprint1.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MohamedSprint1V2.DAL.Database
{
    public class MohamedSprint1V2DbContext : IdentityDbContext<ApplicationUser>
    {
        public MohamedSprint1V2DbContext(DbContextOptions<MohamedSprint1V2DbContext> options) : base(options)
        {
            
        }
        
    
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

    }
}

