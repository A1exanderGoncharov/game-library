using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Game : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string HeaderImage { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Rating { get; set; }
        public string Genre { get; set; }
        public string Trailer { get; set; }
        public string Developer { get; set; }
        //public string Composers { get; set; }

        public ICollection<Comment> Comments { get; set; }
        //public ICollection<ApplicationUser> ApplicationUsers { get; set; }
        //public List<UserGameLibrary> UserGameLibraries { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
