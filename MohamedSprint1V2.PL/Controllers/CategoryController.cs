using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MohamedSprint1V2.DAL.EntityClasses;
using MohamedSprint1V2.DLL.ModelVM.Category;
using MohamedSprint1V2.DLL.ModelVM.Product;
using MohamedSprint1V2.DLL.Service.Abstraction;
using MohamedSprint1V2.DLL.Service.Impelementation;

namespace MohamedSprint1V2.PL.Controllers
{
    [Authorize(Roles = "Manager,Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }
        public IActionResult Index()
        {
            var result = categoryService.getAllCategories();
            if (!result.Successornot)
            {
                TempData["ErrorMessage"] = result.Message; // شوف هنا هيطلعلك السبب الحقيقي
            }
            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new AddCategoryVM()); // مستحسن تبعت instance فاضية
        }

        [HttpPost]
        public IActionResult Create(AddCategoryVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

           

            var response = categoryService.addCategory(model);

            if (response.Successornot)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = response.Message;
            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
           var result=  categoryService.getCategoryById(id);
            if(!result.Successornot)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("Index");
            }
            return View(result.result);

        }
        [HttpPost]
        public IActionResult Edit(UpdaeteCategoryVM categoryVM)
        {
            if(!ModelState.IsValid)
                return View(categoryVM);
            var result=categoryService.updateCategory(categoryVM);
            if (result.Successornot)
            {
                return RedirectToAction("Index");
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

