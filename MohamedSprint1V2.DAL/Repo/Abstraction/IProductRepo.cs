namespace MohamedSprint1V2.DAL.Repo.Abstraction
{
    public interface IProductRepo
    {
        bool AddProduct(Product product);
        bool UpdateProduct(Product product);
        bool DeleteProduct(int id);
        Product GetProductById(int id);
        List<Product> GetAllProducts(Expression<Func<Product, bool>>? filter = null);
    }
}
