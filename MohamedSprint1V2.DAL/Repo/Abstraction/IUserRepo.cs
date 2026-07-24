using System;
using System.Collections.Generic;
using System.Text;

namespace MohamedSprint1V2.DAL.Repo.Abstraction
{
    public interface IUserRepo
    {
        Task<bool> RegisterUserAsync(ApplicationUser user, string password);
        //Task<bool> LoginUserAsync(string username, string password);
        //Task<bool> LogoutUserAsync(string username);
    }
}
