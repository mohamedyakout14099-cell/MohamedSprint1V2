using Microsoft.AspNetCore.Mvc;
using MohamedSprint1V2.DLL.ModelVM.Category;
using MohamedSprint1V2.DLL.ModelVM.Product;
using MohamedSprint1V2.DLL.ModelVM.Pagination;
using MohamedSprint1V2.DLL.Service.Abstraction;

namespace MohamedSprint1V2.PL.Controllers
{
    public class StoreController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public StoreController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult Index(int? categoryId, string? search, string? sort, int pageNumber = 1, int pageSize = 8)
        {
            var productsResponse = _productService.GetAllProducts();
            var categoriesResponse = _categoryService.getAllCategories();

            var products = productsResponse?.result ?? new List<GetAllProductVM>();
            var categories = categoriesResponse?.result ?? new List<GetallCategoryVM>();

            // Filter by Category
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                products = products.Where(p => p.categoryId == categoryId.Value).ToList();
                ViewBag.SelectedCategoryId = categoryId.Value;
            }

            // Filter by Search Term
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                products = products.Where(p => 
                    (p.name != null && p.name.ToLower().Contains(term)) ||
                    (p.description != null && p.description.ToLower().Contains(term))
                ).ToList();
                ViewBag.SearchTerm = search;
            }

            // Sort
            products = sort switch
            {
                "price_asc" => products.OrderBy(p => p.price).ToList(),
                "price_desc" => products.OrderByDescending(p => p.price).ToList(),
                "name_asc" => products.OrderBy(p => p.name).ToList(),
                _ => products
            };
            ViewBag.CurrentSort = sort;
            ViewBag.Categories = categories;

            var pagedProducts = PagedList<GetAllProductVM>.Create(products, pageNumber, pageSize);
            return View(pagedProducts);
        }

        public IActionResult Details(int id)
        {
            var productResponse = _productService.GetProductById(id);
            if (productResponse == null || !productResponse.Successornot || productResponse.result == null)
            {
                TempData["ErrorMessage"] = "Product not found.";
                return RedirectToAction(nameof(Index));
            }

            var categoryResponse = _categoryService.getCategoryById(productResponse.result.CategoryId);
            ViewBag.CategoryName = categoryResponse?.result?.Name ?? "General";

            // Related Products
            var allProductsResponse = _productService.GetAllProducts();
            var related = (allProductsResponse?.result ?? new List<GetAllProductVM>())
                .Where(p => p.categoryId == productResponse.result.CategoryId && p.id != id)
                .Take(4)
                .ToList();
            ViewBag.RelatedProducts = related;

            return View(productResponse.result);
        }
    }
}
