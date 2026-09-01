using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MohamedSprint1V2.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MohamedSprint1V2.DAL.Database
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // 1. Seed Roles (الأدوار)
            string[] roleNames = { "Admin", "User", "Manager" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 2. Seed Default Admin User (مستخدم Admin افتراضي)
            var adminEmail = "admin@admin.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail)
                            ?? await userManager.FindByNameAsync("admin");

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    Name = "System Administrator",
                    EmailConfirmed = true,
                    City = "Cairo",
                    Address = "Main Office",
                    Img = new List<byte>()
                };

                var createResult = await userManager.CreateAsync(adminUser, "Admin@123");
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
            else
            {
                // التأكد من أن الإيميل واسم المستخدم مضبوطين
                if (string.IsNullOrEmpty(adminUser.Email))
                {
                    adminUser.Email = adminEmail;
                    await userManager.UpdateAsync(adminUser);
                }

                // التأكد من إسناد دور Admin
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }

                // التأكد من صحة كلمة المرور وإعادة ضبطها إن لزم
                var isPasswordCorrect = await userManager.CheckPasswordAsync(adminUser, "Admin@123");
                if (!isPasswordCorrect)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(adminUser);
                    await userManager.ResetPasswordAsync(adminUser, token, "Admin@123");
                }
            }
        }
    }
}
