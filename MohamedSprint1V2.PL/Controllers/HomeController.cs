using Microsoft.AspNetCore.Mvc;
using MohamedSprint1V2.DLL.ModelVM.Category;
using MohamedSprint1V2.DLL.ModelVM.Product;
using MohamedSprint1V2.DLL.Service.Abstraction;
using MohamedSprint1V2.PL.Models;
using System.Diagnostics;

namespace MohamedSprint1V2.PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public HomeController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            var productsResponse = _productService.GetAllProducts();
            var categoriesResponse = _categoryService.getAllCategories();

            var products = productsResponse?.result ?? new List<GetAllProductVM>();
            var categories = categoriesResponse?.result ?? new List<GetallCategoryVM>();

            ViewBag.Categories = categories;
            ViewBag.FeaturedProducts = products.Take(8).ToList();
            ViewBag.TotalProductsCount = products.Count;

            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
