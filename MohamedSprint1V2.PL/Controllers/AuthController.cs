using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MohamedSprint1V2.DLL.ModelVM.Identity;
using MohamedSprint1V2.DLL.Service.Abstraction;

namespace MohamedSprint1V2.PL.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [Authorize(Roles = "Manager,Admin")]
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

        [Authorize(Roles = "Manager,Admin")]
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

        [Authorize(Roles = "Manager,Admin")]
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

        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterVM registerVM, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(memoryStream);
                    registerVM.Img = memoryStream.ToArray().ToList();
                }
            }

            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }
            var result = await _authService.Register(registerVM);
            if (result.Successornot)
            {
                // Registration successful, redirect to login
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                // Registration failed, display error message
                ModelState.AddModelError(string.Empty, result.Message ?? "Registration failed.");
                return View(registerVM);
            }
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVm loginVm, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
            {
                return View(loginVm);
            }

            var result = await _authService.Login(loginVm);
            if (result.Successornot) 
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                var normalizedInput = (loginVm.Email ?? "").Trim().ToLower();
                if (normalizedInput == "admin" || normalizedInput == "admin@admin.com" || 
                    normalizedInput == "manager" || normalizedInput == "manager@manager.com")
                {
                    return RedirectToAction("Index", "Auth");
                }

                return RedirectToAction("Index", "Product");
            }

            ModelState.AddModelError(string.Empty, result.Message ?? "Invalid login attempt.");
            return View(loginVm);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}
