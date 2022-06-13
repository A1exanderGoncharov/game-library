using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class ApplicationUserDTO
    {
        public string Id { get; set; }
        public string Nickname { get; set; }
        public string Role { get; set; }

        public ICollection<CollectionDTO> Collections { get; set; }
        //public ICollection<GameDTO> Games { get; set; }
        //public List<UserGameLibraryDTO> UserGameLibraries { get; set; }
    }
}
