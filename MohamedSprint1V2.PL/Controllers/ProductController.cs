using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MohamedSprint1V2.DLL.ModelVM.Category;
using MohamedSprint1V2.DLL.ModelVM.Product;
using MohamedSprint1V2.DLL.Service.Abstraction;

namespace MohamedSprint1V2.PL.Controllers
{
    [Authorize(Roles = "Manager,Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private readonly IFileService fileService;
        private readonly ICategoryService categoryService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductController(
            IProductService productService,
            IFileService fileService,
            ICategoryService categoryService,
            IWebHostEnvironment webHostEnvironment)
        {
            this.productService = productService;
            this.fileService = fileService;
            this.categoryService = categoryService;
            this.webHostEnvironment = webHostEnvironment;
        }

        private void LoadCategories(int? selectedId = null)
        {
            var categoriesResponse = categoryService.getAllCategories();
            var list = categoriesResponse?.result ?? new List<GetallCategoryVM>();
            ViewBag.Categories = new SelectList(list, "id", "name", selectedId);
        }

        //private async Task<string?> UploadImageAsync(IFormFile? imageFile)
        //{
        //    if (imageFile == null || imageFile.Length == 0)
        //        return null;

        //    var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images", "products");
        //    if (!Directory.Exists(uploadsFolder))
        //    {
        //        Directory.CreateDirectory(uploadsFolder);
        //    }

        //    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
        //    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //    using (var fileStream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await imageFile.CopyToAsync(fileStream);
        //    }

        //    return "/images/products/" + uniqueFileName;
        //}

        public IActionResult Index()
        {
            var result = productService.GetAllProducts();
            if (!result.Successornot)
            {
                TempData["ErrorMessage"] = result.Message;
            }
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadCategories();
            return View(new AddProductVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddProductVM model, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadedPath = await fileService.UploadImageAsync(imageFile);
                if (uploadedPath.Successornot)
                {
                    model.Img = uploadedPath.result;
                }
                else
                {
                    ModelState.AddModelError("", uploadedPath.Message ?? "Failed to upload image");
                    LoadCategories(model.CategoryId);

                    return View(model);
                }
            }

            if (string.IsNullOrWhiteSpace(model.Img))
            {
                model.Img = "https://placehold.co/400x300?text=Product";
            }

            if (!ModelState.IsValid)
            {
                LoadCategories(model.CategoryId);
                return View(model);
            }

            var response = productService.AddProduct(model);

            if (response.Successornot)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", response.Message ?? "Failed to add product");
            LoadCategories(model.CategoryId);
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var result = productService.GetProductById(id);
            if (!result.Successornot)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            LoadCategories(result.result.CategoryId);
            return View(result.result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateProductVM productVM, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadedPath = await fileService.UploadImageAsync(imageFile);
                if (uploadedPath.Successornot)
                {
                    await fileService.DeleteImageAsync(productVM.Img);
                    productVM.Img = uploadedPath.result;
                }
            }

            var old = productService.GetProductById(productVM.Id);
            if (!old.Successornot)
            {
                TempData["ErrorMessage"] = old.Message;
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(productVM.Img))
            {
                productVM.Img = old.result.Img;
            }

            if (!ModelState.IsValid)
            {
                LoadCategories(productVM.CategoryId);
                return View(productVM);
            }

            var result = productService.UpdateProduct(productVM);
            if (result.Successornot)
            {
                TempData["SuccessMessage"] = "Product updated successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
                LoadCategories(productVM.CategoryId);
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
        [ValidateAntiForgeryToken]
        public IActionResult Delete(UpdateProductVM productVM)
        {
            fileService.DeleteImageAsync(productVM.Img);
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