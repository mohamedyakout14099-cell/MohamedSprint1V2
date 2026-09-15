using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MohamedSprint1V2.DLL.ModelVM.Category;
using MohamedSprint1V2.DLL.ModelVM.Pagination;
using MohamedSprint1V2.DLL.ModelVM.ResponseResult;
using MohamedSprint1V2.DLL.Service.Abstraction;

namespace MohamedSprint1V2.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Manager,Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public IActionResult Index(string? search, int pageNumber = 1, int pageSize = 10)
        {
            var result = categoryService.getAllCategories();
            if (!result.Successornot)
            {
                TempData["ErrorMessage"] = result.Message;
            }

            var list = result?.result ?? new List<GetallCategoryVM>();
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                list = list.Where(c => 
                    (c.name != null && c.name.ToLower().Contains(term)) ||
                    (c.description != null && c.description.ToLower().Contains(term))
                ).ToList();
                ViewBag.SearchTerm = search;
            }

            var pagedList = PagedList<GetallCategoryVM>.Create(list, pageNumber, pageSize);
            var response = new Response<PagedList<GetallCategoryVM>>(pagedList, result?.Message, result?.Successornot ?? false);
            return View(response);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new AddCategoryVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AddCategoryVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = categoryService.addCategory(model);

            if (response.Successornot)
            {
                TempData["SuccessMessage"] = response.Message ?? "Category created successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = response.Message;
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var result = categoryService.getCategoryById(id);
            if (!result.Successornot)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }
            return View(result.result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UpdaeteCategoryVM categoryVM)
        {
            if (!ModelState.IsValid)
                return View(categoryVM);

            var result = categoryService.updateCategory(categoryVM);
            if (result.Successornot)
            {
                TempData["SuccessMessage"] = "Category updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
                return View(categoryVM);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var result = categoryService.getCategoryById(id);
            if (!result.Successornot)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            return View(result.result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(UpdaeteCategoryVM categoryVM)
        {
            var result = categoryService.deleteCategoryById(categoryVM.Id);

            if (result.Successornot)
            {
                TempData["SuccessMessage"] = "Category Deleted Successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.Message;
            return View(categoryVM);
        }
    }
}
