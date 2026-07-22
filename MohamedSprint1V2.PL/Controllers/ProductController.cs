using Microsoft.AspNetCore.Mvc;
using MohamedSprint1V2.DLL.ModelVM.Product;
using MohamedSprint1V2.DLL.Service.Abstraction;
using MohamedSprint1V2.DLL.Service.Impelementation;
namespace MohamedSprint1V2.PL.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }
        public IActionResult Index()
        {
            var result = productService.GetAllProducts();
            if (!result.Successornot)
            {
                TempData["ErrorMessage"] = result.Message; // شوف هنا هيطلعلك السبب الحقيقي
            }
            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new AddProductVM()); // مستحسن تبعت instance فاضية
        }
        [HttpPost]
        public IActionResult Create(AddProductVM model)
        {
            var response = productService.AddProduct(model);

            Console.WriteLine($"Success = {response.Successornot}");
            Console.WriteLine($"Message = {response.Message}");

            if (response.Successornot)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", response.Message);

            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var result = productService.GetProductById(id);
            if (!result.Successornot)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("Index");
            }
            return View(result.result);
        }
        [HttpPost]
        public IActionResult Edit(UpdateProductVM productVM)
        {
            var old = productService.GetProductById(productVM.Id);
            if(!old.Successornot)
            {
                TempData["ErrorMessage"] = old.Message;
                return RedirectToAction("Index");
            }
            var result = productService.UpdateProduct(productVM);
            if (result.Successornot)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
                return View(productVM);
            }
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var result = productService.GetProductById(id);
            if (!result.Successornot)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }
            return View(result.result);
        }
        [HttpPost]
        public IActionResult Delete(UpdateProductVM productVM)
        {
            var result = productService.DeleteProduct(productVM.Id);

            if (result.Successornot)
            {
                TempData["SuccessMessage"] = "Product Deleted Successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.Message;
            return View(productVM);
        }
    }
}