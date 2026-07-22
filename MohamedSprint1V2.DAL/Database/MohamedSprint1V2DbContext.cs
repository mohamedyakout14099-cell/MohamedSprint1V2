using Microsoft.EntityFrameworkCore;
//using MohamedSprint1.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MohamedSprint1.DAL.Database
{
    public class MohamedSprint1V2DbContext : DbContext
    {
        public MohamedSprint1V2DbContext(DbContextOptions<MohamedSprint1V2DbContext> options) : base(options)
        {
            
        }

        //override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=MohamedSprint1V2;Trusted_Connection=True;TrustServerCertificate=True;");
        //}
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}

