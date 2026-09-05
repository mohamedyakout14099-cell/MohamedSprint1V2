using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MohamedSprint1V2.DLL.Service.Abstraction;

namespace MohamedSprint1V2.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Manager,Admin")]
    public class UserController : Controller
    {
        private readonly IAuthService _authService;

        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _authService.GetAllUSers();
            return View(users);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeAdmin(string userId)
        {
            var result = await _authService.MakeAdmin(userId);
            if (result.Successornot)
            {
                TempData["SuccessMessage"] = "User successfully promoted to Admin.";
            }
            else
            {
                TempData["ErrorMessage"] = result.Message ?? "Failed to promote user to Admin.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SoftDelete(string userId)
        {
            var result = await _authService.SoftDeleteUser(userId);
            if (result.Successornot)
            {
                TempData["SuccessMessage"] = "User account deactivated (soft-deleted).";
            }
            else
            {
                TempData["ErrorMessage"] = result.Message ?? "Failed to deactivate user.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(string userId)
        {
            var result = await _authService.RestoreUser(userId);
            if (result.Successornot)
            {
                TempData["SuccessMessage"] = "User account restored successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = result.Message ?? "Failed to restore user.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
