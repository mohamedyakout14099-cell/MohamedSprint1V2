using MohamedSprint1V2.DLL.ModelVM.Product;

namespace MohamedSprint1V2.DLL.Service.Abstraction
{
    public interface IProductService
    {
        Response<bool> AddProduct(AddProductVM productVM);
        Response<bool> UpdateProduct(UpdateProductVM productVM);
        Response<bool> DeleteProduct(int id);
        Response<bool> SoftDeleteProduct(int id);
        Response<bool> RestoreProduct(int id);
        Response<List<GetAllProductVM>> GetAllProducts();
        Response<List<GetAllProductVM>> GetAllProductsIncludingDeleted();
        Response<UpdateProductVM> GetProductById(int id);
        Response<UpdateProductVM> GetProductByIdIncludingDeleted(int id);
    }
}
