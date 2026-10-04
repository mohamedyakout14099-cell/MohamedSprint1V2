using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MohamedSprint1V2.DLL.ModelVM.Review;
using MohamedSprint1V2.DLL.Service.Abstraction;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MohamedSprint1V2.PL.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        // POST: /Review/Add
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddReviewVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please provide a valid rating (1-5) and review comment.";
                return RedirectToAction("Details", "Store", new { id = model.ProductId });
            }

            var userId = GetUserId();
            var response = await _reviewService.AddReviewAsync(model, userId);

            if (response.Successornot)
            {
                TempData["SuccessMessage"] = response.Message;
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
            }

            return RedirectToAction("Details", "Store", new { id = model.ProductId });
        }

        // POST: /Review/Delete
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int productId)
        {
            var userId = GetUserId();
            var isAdmin = User.IsInRole("Admin");

            var response = await _reviewService.DeleteReviewAsync(id, userId, isAdmin);

            if (response.Successornot)
            {
                TempData["SuccessMessage"] = response.Message;
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
            }

            return RedirectToAction("Details", "Store", new { id = productId });
        }
    }
}
