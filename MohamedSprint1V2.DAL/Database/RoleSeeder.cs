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

            // 3. Seed Default Manager User (مستخدم Manager افتراضي)
            var managerEmail = "manager@manager.com";
            var managerUser = await userManager.FindByEmailAsync(managerEmail)
                              ?? await userManager.FindByNameAsync("manager");

            if (managerUser == null)
            {
                managerUser = new ApplicationUser
                {
                    UserName = "manager",
                    Email = managerEmail,
                    Name = "General Manager",
                    EmailConfirmed = true,
                    City = "Cairo",
                    Address = "Headquarters",
                    Img = new List<byte>()
                };

                var createMgrResult = await userManager.CreateAsync(managerUser, "Manager@123");
                if (createMgrResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(managerUser, "Manager");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(managerUser.Email))
                {
                    managerUser.Email = managerEmail;
                    await userManager.UpdateAsync(managerUser);
                }

                if (!await userManager.IsInRoleAsync(managerUser, "Manager"))
                {
                    await userManager.AddToRoleAsync(managerUser, "Manager");
                }

                var isMgrPasswordCorrect = await userManager.CheckPasswordAsync(managerUser, "Manager@123");
                if (!isMgrPasswordCorrect)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(managerUser);
                    await userManager.ResetPasswordAsync(managerUser, token, "Manager@123");
                }
            }
        }
    }
}
