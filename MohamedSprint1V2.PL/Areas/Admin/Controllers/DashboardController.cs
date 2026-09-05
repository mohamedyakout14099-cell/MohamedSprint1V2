using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MohamedSprint1V2.DLL.ModelVM.Identity;
using MohamedSprint1V2.DLL.ModelVM.Product;
using MohamedSprint1V2.DLL.Service.Abstraction;

namespace MohamedSprint1V2.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Manager,Admin")]
    public class DashboardController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IAuthService _authService;

        public DashboardController(
            IProductService productService,
            ICategoryService categoryService,
            IAuthService authService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _authService = authService;
        }

        public async Task<IActionResult> Index()
        {
            var productsResult = _productService.GetAllProducts();
            var categoriesResult = _categoryService.getAllCategories();
            var usersResponse = await _authService.GetAllUSers();

            var productsList = productsResult?.result ?? new List<GetAllProductVM>();
            var categoriesList = categoriesResult?.result ?? new List<DLL.ModelVM.Category.GetallCategoryVM>();
            var usersList = usersResponse?.result?.ToList() ?? new List<AllUserVM>();

            ViewBag.TotalProducts = productsList.Count;
            ViewBag.TotalCategories = categoriesList.Count;
            ViewBag.TotalUsers = usersList.Count;
            ViewBag.RecentProducts = productsList.Take(5).ToList();

            return View();
        }
    }
}
