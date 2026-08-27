using MohamedSprint1V2.DLL.ModelVM.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MohamedSprint1V2.DLL.Service.Abstraction
{
    public interface IAuthService
    {
        Task<Response<bool>> Register(RegisterVM registerVM);
        Task<Response<bool>> Login(LoginVm loginVm);
    }
}
