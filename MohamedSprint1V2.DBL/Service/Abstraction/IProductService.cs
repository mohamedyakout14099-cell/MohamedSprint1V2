using MohamedSprint1V2.DLL.ModelVM.Product;

namespace MohamedSprint1V2.DLL.Service.Abstraction
{
    public interface IProductService
    {
        Response<bool> AddProduct(AddProductVM productVM);
        Response<bool> UpdateProduct(UpdateProductVM productVM);
        Response<bool> DeleteProduct(int id);
        Response<List<GetAllProductVM>> GetAllProducts();
        Response<UpdateProductVM> GetProductById(int id);
    }
}
