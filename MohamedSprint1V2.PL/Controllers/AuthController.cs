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

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterVM registerVM)
        {
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
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVm loginVm)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVm);
            }

            var result = await _authService.Login(loginVm);
            if (result.Successornot) 
            {
                return RedirectToAction("Index", "Product");
            }

            ModelState.AddModelError(string.Empty, result.Message ?? "Invalid login attempt.");
            return View(loginVm);
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
