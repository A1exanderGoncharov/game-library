using BLL.DTO;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserService
    {
        Task<SignInResult> LoginAsync(UserLoginModel userLoginModel);
        Task<IdentityResult> RegisterAsync(UserRegisterModel userRegisterModel);
        IEnumerable<ApplicationUserDTO> GetAllUsers();
        ApplicationUserDTO GetUserById(string targetUserId);
        IEnumerable<IdentityRole> GetRoles();
        Task SignOutAsync();
    }
}
