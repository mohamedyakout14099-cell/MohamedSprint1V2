using MohamedSprint1V2.DLL.ModelVM.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MohamedSprint1V2.DLL.Service.Abstraction
{
    public interface IAuthService
    {
        Task<Response<IEnumerable<AllUserVM>>> GetAllUSers();
        Task<Response<bool>> Register(RegisterVM registerVM);
        Task<Response<bool>> Login(LoginVm loginVm);
        Task Logout();
        Task<Response<bool>> SoftDeleteUser(string userId);
        Task<Response<bool>> RestoreUser(string userId);
        Task<Response<bool>> MakeAdmin(string userId);
    }
}
