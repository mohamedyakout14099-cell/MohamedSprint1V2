using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MohamedSprint1V2.DAL.Entity;
using MohamedSprint1V2.DLL.ModelVM.Identity;
using MohamedSprint1V2.DLL.Service.Abstraction;

namespace MohamedSprint1V2.PL.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(IAuthService authService, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpGet]
        public IActionResult Index()
        {
            // Redirect to the dedicated Admin Area user management
            return RedirectToAction("Index", "User", new { area = "Admin" });
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
                return View("Register", registerVM);
            }

            var result = await _authService.Register(registerVM);
            if (result.Successornot)
            {
                TempData["SuccessMessage"] = "Registration successful! Please login to your account.";
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "Registration failed.");
                return View("Register", registerVM);
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

                // Check logged in user's roles
                var user = await _userManager.FindByEmailAsync(loginVm.Email) 
                           ?? await _userManager.FindByNameAsync(loginVm.Email);

                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("Admin") || roles.Contains("Manager"))
                    {
                        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                    }
                }

                // Regular customer redirection
                return RedirectToAction("Index", "Home");
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
