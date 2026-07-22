using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MohamedSprint1V2.DAL.Repo.Impelementation
{
    public class ProductRepo : IProductRepo
    {
        private MohamedSprint1V2DbContext Db;
        public ProductRepo(MohamedSprint1V2DbContext Db)
        {
             this.Db = Db;
        }
        public bool AddProduct(Product product)
        {
          
                if(product != null)
                {
                    Db.Products.Add(product);
                    Db.SaveChanges();

                    return true;
                }
                return false;
            
        }

        public bool DeleteProduct(int id)
        {
            var product = Db.Products.Find(id);
            if (product != null)
            {
                Db.Products.Remove(product);
                Db.SaveChanges();
                return true;
            }
            return false;
        }

      
        public List<Product> GetAllProducts(Expression<Func<Product, bool>>? filter = null)
        {
            if (filter == null)
            {
                return Db.Products.ToList();
            }
            return Db.Products.Where(filter).ToList();
        }

        public Product GetProductById(int id)
        {
            return Db.Products.Find(id);
        }

        public bool UpdateProduct(Product product)
        //{
        //    var old = Db.Products.Find(product.Id);
        //    if (old != null)
        //    {
        //        old.update(product.Name, product.Description,product.Img, product.Price,product.CategoryId);
        //        Db.SaveChanges();
        //        return true;
        //    }
        //    return false;
    //public bool UpdateProduct(Product product)
    //    {
    //        var old = Db.Products.Find(product.Id);

    //        if (old != null)
    //        {
    //            Console.WriteLine($"Old Name Before = {old.Name}");
    //            Console.WriteLine($"New Name = {product.Name}");

    //            old.update(product.Name,
    //                       product.Description,
    //                       product.Img,
    //                       product.Price,
    //                       product.CategoryId);

    //            Console.WriteLine($"Old Name After = {old.Name}");

    //            Db.SaveChanges();

    //            return true;
    //        }

    //        return false;
    //    }public bool UpdateProduct(Product product)
{
    var old = Db.Products.Find(product.Id);

    if (old == null)
    {
        Console.WriteLine("Not Found");
        return false;
    }

    //Console.WriteLine($"Before: {old.Name}");

    old.update(
        product.Name,
        product.Description,
        product.Img,
        product.Price,
        product.CategoryId);
            Db.SaveChanges();

    //Console.WriteLine($"update() returned = {updated}");
    //Console.WriteLine($"After: {old.Name}");

    //var rows = Db.SaveChanges();

    //Console.WriteLine($"Rows = {rows}");

    return true;
}
    }
}
//}
