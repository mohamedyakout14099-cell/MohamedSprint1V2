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
        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<bool> LoginUserAsync(
             string userName,
             string password,
             bool RememberMe)
        {
            var user = await _userManager.FindByEmailAsync(userName)
                       ?? await _userManager.FindByNameAsync(userName);

            if (user == null || string.IsNullOrEmpty(user.UserName) || user.IsDeleted)
            {
                return false;
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
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

        public async Task<bool> SoftDeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.IsDeleted = true;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> RestoreUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.IsDeleted = false;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> MakeAdminAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Contains("User"))
            {
                await _userManager.RemoveFromRoleAsync(user, "User");
            }

            if (!currentRoles.Contains("Admin"))
            {
                var addResult = await _userManager.AddToRoleAsync(user, "Admin");
                return addResult.Succeeded;
            }

            return true;
        }

        public async Task<string> GetUserRoleAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Manager")) return "Manager";
            if (roles.Contains("Admin")) return "Admin";
            if (roles.Contains("User")) return "User";
            return roles.FirstOrDefault() ?? "User";
        }

      
    }
}
