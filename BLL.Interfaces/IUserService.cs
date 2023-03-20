using BLL.DTO;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserService
    {
        Task<SignInResult> Login(UserLoginModel userLoginModel);
        Task<IdentityResult> Register(UserRegisterModel userRegisterModel);
        IEnumerable<IdentityRole> GetRoles();
        Task SignOut();
    }
}
