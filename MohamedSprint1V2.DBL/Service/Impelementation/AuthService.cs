using MohamedSprint1V2.DAL.Entity;
using MohamedSprint1V2.DLL.ModelVM.Identity;
using System;
using System.Threading.Tasks;

namespace MohamedSprint1V2.DLL.Service.Impelementation
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepo _userRepo;
        private readonly IEmailService _emailService;

        public AuthService(IUserRepo userRepo, IEmailService emailService)
        {
            _userRepo = userRepo;
            _emailService = emailService;
        }
        public async Task<Response<IEnumerable<AllUserVM>>> GetAllUSers()
        {
            try
            {
                var users = await _userRepo.GetAllUsersAsync();
                if (users != null)
                {
                    var userVmList = new List<AllUserVM>();
                    foreach (var u in users)
                    {
                        var role = await _userRepo.GetUserRoleAsync(u);
                        userVmList.Add(new AllUserVM
                        {
                            Id = u.Id,
                            Name = u.Name,
                            UserName = u.UserName,
                            Email = u.Email,
                            Address = u.Address,
                            City = u.City,
                            Img = u.Img,
                            IsDeleted = u.IsDeleted,
                            Role = role
                        });
                    }
                    return new Response<IEnumerable<AllUserVM>>(userVmList, "Users retrieved successfully.", true);
                }
                return new Response<IEnumerable<AllUserVM>>(null, "No users found.", false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<AllUserVM>>(null, ex.Message, false);
            }
        }

        public async Task<Response<IEnumerable<AllUserVM>>> GetAllUsersIncludingDeleted()
        {
            try
            {
                var users = await _userRepo.GetAllUsersIncludingDeletedAsync();
                if (users != null)
                {
                    var userVmList = new List<AllUserVM>();
                    foreach (var u in users)
                    {
                        var role = await _userRepo.GetUserRoleAsync(u);
                        userVmList.Add(new AllUserVM
                        {
                            Id = u.Id,
                            Name = u.Name,
                            UserName = u.UserName,
                            Email = u.Email,
                            Address = u.Address,
                            City = u.City,
                            Img = u.Img,
                            IsDeleted = u.IsDeleted,
                            Role = role
                        });
                    }
                    return new Response<IEnumerable<AllUserVM>>(userVmList, "Users retrieved successfully.", true);
                }
                return new Response<IEnumerable<AllUserVM>>(null, "No users found.", false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<AllUserVM>>(null, ex.Message, false);
            }
        }

        public async Task<Response<bool>> Login(LoginVm loginVm)
        {
            var result = await _userRepo.LoginUserAsync(loginVm.Email, loginVm.Password, loginVm.RememberMe);
            if (result)
            {
                return new Response<bool>(true, "User Logged in successfully.", true);
            }
            return new Response<bool>(false, "Failed to Log in user or account is inactive.", false);
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
                    registerVM.Email,
                    registerVM.Img
                );

                var result = await _userRepo.RegisterUserAsync(newUser, registerVM.Password);

                if (result)
                {
                    // Fire-and-forget welcome email so registration is never delayed
                    if (!string.IsNullOrEmpty(newUser.Email))
                    {
                        _ = _emailService.SendWelcomeEmailAsync(newUser.Email, newUser.Name ?? newUser.UserName ?? "Customer");
                    }

                    return new Response<bool>(true, "User registered successfully.", true);
                }

                return new Response<bool>(false, "Failed to register user.", false);
            }
            catch (Exception ex)
            {
                return new Response<bool>(false, ex.Message, false);
            }
        }

        public async Task Logout()
        {
            await _userRepo.LogoutUserAsync();
        }

        public async Task<Response<bool>> SoftDeleteUser(string userId)
        {
            try
            {
                var result = await _userRepo.SoftDeleteUserAsync(userId);
                if (result)
                {
                    return new Response<bool>(true, "User soft-deleted successfully.", true);
                }
                return new Response<bool>(false, "Failed to soft-delete user.", false);
            }
            catch (Exception ex)
            {
                return new Response<bool>(false, ex.Message, false);
            }
        }

        public async Task<Response<bool>> RestoreUser(string userId)
        {
            try
            {
                var result = await _userRepo.RestoreUserAsync(userId);
                if (result)
                {
                    return new Response<bool>(true, "User restored successfully.", true);
                }
                return new Response<bool>(false, "Failed to restore user.", false);
            }
            catch (Exception ex)
            {
                return new Response<bool>(false, ex.Message, false);
            }
        }

        public async Task<Response<bool>> MakeAdmin(string userId)
        {
            try
            {
                var result = await _userRepo.MakeAdminAsync(userId);
                if (result)
                {
                    return new Response<bool>(true, "User promoted to Admin successfully.", true);
                }
                return new Response<bool>(false, "Failed to promote user to Admin.", false);
            }
            catch (Exception ex)
            {
                return new Response<bool>(false, ex.Message, false);
            }
        }
    }
}

