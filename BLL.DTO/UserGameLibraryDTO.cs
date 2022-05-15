using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class UserGameLibraryDTO
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public GameDTO Game { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUserDTO ApplicationUser { get; set; }
        public bool IsPassed { get; set; }
    }
}
