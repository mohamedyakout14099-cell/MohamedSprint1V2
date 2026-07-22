using MohamedSprint1V2.DAL.Entity;
namespace MohamedSprint1V2.DLL.Service.Impelementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo productRepo;
        public ProductService(IProductRepo productRepo)
        {
            this.productRepo = productRepo;
        }
        public Response<bool> AddProduct(AddProductVM productVM)
        {
            try
            {
                var product = new Product(productVM.Name,productVM.Description,productVM.Img,productVM.Price,productVM.CategoryId);
                var result = productRepo.AddProduct(product);
                if (result)
                {
                    return new Response<bool>(true,"Product added successfully",true);
                }
                else
                {
                    return new Response<bool>(false,"Failed to add product",false);
                }
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return new Response<bool>(false, message, false);
            }
        }
        public Response<bool> DeleteProduct(int id)
        {
            try
            {
                var result = productRepo.DeleteProduct(id);
                if (result)
                {
                    return new Response<bool>(true, "Product deleted successfully", true);
                }
                else
                {
                    return new Response<bool>(false, "Failed to delete product", false);
                }

            }
            catch (Exception ex)
            {
                return new Response<bool>(false, $"An error occurred: {ex.Message}", false);
            }
        }
        public Response<List<GetAllProductVM>> GetAllProducts()
        {
            try
            {
                var result = productRepo.GetAllProducts();

                if (result == null || result.Count == 0)
                {
                    return new Response<List<GetAllProductVM>>
                    (
                        null,
                        "No Products Found",
                        false
                    );
                }

                List<GetAllProductVM> mapp = new List<GetAllProductVM>();

                foreach (var item in result)
                {
                    mapp.Add(new GetAllProductVM()
                    {
                        id = item.Id,
                        name = item.Name,
                        description = item.Description,
                        img = item.Img,
                        price = item.Price,
                        categoryId = item.CategoryId
                    });
                }

                return new Response<List<GetAllProductVM>>
                (
                    mapp,
                    "Products retrieved successfully",
                    true
                );
            }
            catch (Exception ex)
            {
                return new Response<List<GetAllProductVM>>
                (
                    null,
                    $"An error occurred: {ex.Message}",
                    false
                );
            }
        }
        public Response<UpdateProductVM> GetProductById(int id)
        {
            try
            {
                var product = productRepo.GetProductById(id);
                if (product != null)
                {
                    var productVM = new UpdateProductVM
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Img = product.Img,
                        Price = product.Price,
                        CategoryId = product.CategoryId
                    };
                    return new Response<UpdateProductVM>(productVM, "Product retrieved successfully", true);
                }
                else
                {
                    return new Response<UpdateProductVM>(null, "Product not found", false);
                }

            }
            catch (Exception ex)
            {
                return new Response<UpdateProductVM>(null, $"An error occurred: {ex.Message}", false);
            }
        }
        public Response<bool> UpdateProduct(UpdateProductVM productVM)
        {

            try
            {
                var Product = new Product(productVM.Id, productVM.Name, productVM.Description, productVM.Img, productVM.Price, productVM.CategoryId);
                var result = productRepo.UpdateProduct(Product);
                if(result)
                {
                    return new Response<bool>(true, "Product updated successfully", true);
                }
                else
                {
                    return new Response<bool>(false, "Failed to update product", false);
                }
            }
            catch(Exception ex)
            {
                return new Response<bool>(false, $"An error occurred: {ex.Message}", false);
            }
        }
    }
}
