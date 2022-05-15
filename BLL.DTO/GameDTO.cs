using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class GameDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string HeaderImage { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime ReleaseDate { get; set; }
        public string Rating { get; set; }
        public string Genre { get; set; }
        public string Trailer { get; set; }
        public string Developer { get; set; }
        //public string Composers { get; set; }

        public ICollection<CommentDTO> Comments { get; set; }
        //public ICollection<ApplicationUserDTO> ApplicationUsers { get; set; }
        //public List<UserGameLibraryDTO> UserGameLibraries { get; set; }
        
        public ApplicationUserDTO ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }

    }
}
