using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MohamedSprint1V2.DLL.ModelVM.Category;
using MohamedSprint1V2.DLL.ModelVM.Product;
using MohamedSprint1V2.DLL.ModelVM.Pagination;
using MohamedSprint1V2.DLL.ModelVM.ResponseResult;
using MohamedSprint1V2.DLL.Service.Abstraction;

namespace MohamedSprint1V2.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Manager,Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private readonly IFileService fileService;
        private readonly ICategoryService categoryService;

        public ProductController(
            IProductService productService,
            IFileService fileService,
            ICategoryService categoryService)
        {
            this.productService = productService;
            this.fileService = fileService;
            this.categoryService = categoryService;
        }

        private void LoadCategories(int? selectedId = null)
        {
            var categoriesResponse = categoryService.getAllCategories();
            var list = categoriesResponse?.result ?? new List<GetallCategoryVM>();
            ViewBag.Categories = new SelectList(list, "id", "name", selectedId);
        }

        public IActionResult Index(string? search, int pageNumber = 1, int pageSize = 10)
        {
            var result = productService.GetAllProducts();
            if (!result.Successornot)
            {
                TempData["ErrorMessage"] = result.Message;
            }

            var list = result?.result ?? new List<GetAllProductVM>();
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                list = list.Where(p => 
                    (p.name != null && p.name.ToLower().Contains(term)) ||
                    (p.description != null && p.description.ToLower().Contains(term))
                ).ToList();
                ViewBag.SearchTerm = search;
            }

            var pagedList = PagedList<GetAllProductVM>.Create(list, pageNumber, pageSize);
            var response = new Response<PagedList<GetAllProductVM>>(pagedList, result?.Message, result?.Successornot ?? false);
            return View(response);
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
                TempData["SuccessMessage"] = response.Message ?? "Product added successfully!";
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
                    if (!string.IsNullOrEmpty(productVM.Img))
                    {
                        await fileService.DeleteImageAsync(productVM.Img);
                    }
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
        public async Task<IActionResult> Delete(UpdateProductVM productVM)
        {
            if (!string.IsNullOrEmpty(productVM.Img))
            {
                await fileService.DeleteImageAsync(productVM.Img);
            }
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
