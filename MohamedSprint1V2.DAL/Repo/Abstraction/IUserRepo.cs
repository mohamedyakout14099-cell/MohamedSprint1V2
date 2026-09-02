using System;
using System.Collections.Generic;
using System.Text;

namespace MohamedSprint1V2.DAL.Repo.Abstraction
{
    public interface IUserRepo
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<bool> RegisterUserAsync(ApplicationUser user, string password);
        Task<bool> LoginUserAsync(string username, string password, bool RememberMe);
        Task LogoutUserAsync();
        Task<bool> SoftDeleteUserAsync(string userId);
        Task<bool> RestoreUserAsync(string userId);
        Task<bool> MakeAdminAsync(string userId);
        Task<string> GetUserRoleAsync(ApplicationUser user);
    }
}
