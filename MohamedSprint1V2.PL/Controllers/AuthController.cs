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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _authService.GetAllUSers();
            return View(users);
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
                if(loginVm.UserName == "admin")
                    return RedirectToAction("Index", "Auth");
                else
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
