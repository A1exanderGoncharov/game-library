using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Infrastructure
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SignInResult> LoginAsync(UserLoginModel userLoginModel)
        {
            var result = await _unitOfWork.SignInManager
                .PasswordSignInAsync(userLoginModel.Email, userLoginModel.Password, userLoginModel.RememberMe, false);

            return result;
        }

        public async Task<IdentityResult> RegisterAsync(UserRegisterModel userRegisterModel)
        {
            var user = _mapper.Map<UserRegisterModel, ApplicationUser>(userRegisterModel);
            var result = await _unitOfWork.UserManager.CreateAsync(user, userRegisterModel.Password);

            if (result.Succeeded)
            {
                await _unitOfWork.UserManager.AddToRoleAsync(user, userRegisterModel.Role);
                await _unitOfWork.SignInManager.SignInAsync(user, false);
                return result;
            }

            else return result;
        }

        public IEnumerable<IdentityRole> GetRoles()
        {
            var roles = _unitOfWork.RoleManager.Roles;
            return roles.ToList();
        }

        public async Task SignOutAsync()
        {
            await _unitOfWork.SignInManager.SignOutAsync();
        }

        public IEnumerable<ApplicationUserDTO> GetAllUsers()
        {
            var usersEntities = _unitOfWork.UserRepository.GetAllAsync().ToList();

            return _mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<ApplicationUserDTO>>(usersEntities);
        }

        public ApplicationUserDTO GetUserById(string targetUserId)
        {
            var targetUser = _unitOfWork.UserRepository.GetAllAsync().FirstOrDefault(u => u.Id == targetUserId)
                ?? throw new Exception("User does not exist!");

            return _mapper.Map<ApplicationUser, ApplicationUserDTO>(targetUser);
        }
    }
}
