using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MohamedSprint1V2.DLL.ModelVM.Identity;
using MohamedSprint1V2.DLL.ModelVM.Pagination;
using MohamedSprint1V2.DLL.ModelVM.ResponseResult;
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
        public async Task<IActionResult> Index(string? search, int pageNumber = 1, int pageSize = 10)
        {
            var usersResponse = await _authService.GetAllUSers();
            var list = usersResponse?.result?.ToList() ?? new List<AllUserVM>();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                list = list.Where(u =>
                    (u.Name != null && u.Name.ToLower().Contains(term)) ||
                    (u.Email != null && u.Email.ToLower().Contains(term)) ||
                    (u.Role != null && u.Role.ToLower().Contains(term)) ||
                    (u.City != null && u.City.ToLower().Contains(term))
                ).ToList();
                ViewBag.SearchTerm = search;
            }

            var pagedList = PagedList<AllUserVM>.Create(list, pageNumber, pageSize);
            var response = new Response<PagedList<AllUserVM>>(pagedList, usersResponse?.Message, usersResponse?.Successornot ?? false);
            return View(response);
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
