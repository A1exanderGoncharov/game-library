using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Nickname { get; set; }
        public string Role { get; set; }

        public ICollection<Collection> Collections { get; set; }
        //public ICollection<Game> Games { get; set; }
        //public List<UserGameLibrary> UserGameLibraries { get; set; }
    }
}
