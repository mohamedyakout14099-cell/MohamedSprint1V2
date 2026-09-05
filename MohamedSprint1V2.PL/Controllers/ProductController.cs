using Microsoft.AspNetCore.Mvc;

namespace MohamedSprint1V2.PL.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            if (User.IsInRole("Admin") || User.IsInRole("Manager"))
            {
                return RedirectToAction("Index", "Product", new { area = "Admin" });
            }
            return RedirectToAction("Index", "Store");
        }

        public IActionResult Details(int id)
        {
            return RedirectToAction("Details", "Store", new { id });
        }
    }
}