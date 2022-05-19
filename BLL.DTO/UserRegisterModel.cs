using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class UserRegisterModel
    {
        [Required(ErrorMessage = "Nickname is required")]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords are not equal")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        //[Required]
        [Display(Name = "User Role")]
        public string UserRole { get; set; }

        public List<IdentityRole> AllRoles { get; set; }
    }
}
