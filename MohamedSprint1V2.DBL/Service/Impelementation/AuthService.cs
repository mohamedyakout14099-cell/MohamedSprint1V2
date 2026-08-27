using MohamedSprint1V2.DAL.Entity;
using MohamedSprint1V2.DLL.ModelVM.Identity;
using System;
using System.Threading.Tasks;

namespace MohamedSprint1V2.DLL.Service.Impelementation
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepo _userRepo;

        public AuthService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<Response<bool>> Login(LoginVm loginVm)
        {
            var result=await _userRepo.LoginUserAsync(loginVm.Email, loginVm.Password, loginVm.RememberMe);
            if (result)
            {
                return new Response<bool>(true, "User Loeged in successfully.", true);

            }
                return new Response<bool>(false, "Failed to Log in user.", false);
        }

        public async Task<Response<bool>> Register(RegisterVM registerVM)
        {
            try
            {
                var newUser = new ApplicationUser(
                    registerVM.Name,
                    registerVM.Address,
                    registerVM.City,
                    registerVM.UserName,
                    registerVM.Email
                );

                var result = await _userRepo.RegisterUserAsync(newUser, registerVM.Password);

                if (result)
                {
                    return new Response<bool>(true, "User registered successfully.", true);
                }

                return new Response<bool>(false, "Failed to register user.", false);
            }
            catch (Exception ex)
            {
                return new Response<bool>(false, ex.Message, false);
            }
        }
    }
}

