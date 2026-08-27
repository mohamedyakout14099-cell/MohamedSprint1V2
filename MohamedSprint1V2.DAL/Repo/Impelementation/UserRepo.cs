using System;
using System.Collections.Generic;
using System.Text;

namespace MohamedSprint1V2.DAL.Repo.Impelementation
{
    public class UserRepo: IUserRepo
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRepo(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<bool> LoginUserAsync(
             string userName,
             string password,
             bool RememberMe)
        {
            var result = await _signInManager.PasswordSignInAsync(
                userName,
                password,
                RememberMe,
                false);
 
            return result.Succeeded;
        }

        public async Task LogoutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }

     

        public async Task<bool> RegisterUserAsync(ApplicationUser user, string password)
        {
            //user.UserName = user.Email;
            var result = await _userManager.CreateAsync(user, password); 

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Error: {error.Code}");
                    Console.WriteLine($"Error: {error.Description}");
                }
                return false;
            }

            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }

            await _userManager.AddToRoleAsync(user, "User");

            return result.Succeeded;
        }

      
    }
}
