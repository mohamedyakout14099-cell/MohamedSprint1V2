using Microsoft.AspNetCore.Mvc;

namespace MohamedSprint1V2.PL.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index(int? id)
        {
            if (User.IsInRole("Admin") || User.IsInRole("Manager"))
            {
                return RedirectToAction("Index", "Category", new { area = "Admin" });
            }
            return RedirectToAction("Index", "Store", new { categoryId = id });
        }
    }
}
