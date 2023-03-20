using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public class Game : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string HeaderImage { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public string Rating { get; set; }
        public string Trailer { get; set; }
        public string Developer { get; set; }

        public ICollection<Comment> Comments { get; set; }
        //public ICollection<ApplicationUser> ApplicationUsers { get; set; }
        //public List<UserGameLibrary> UserGameLibraries { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }

        //public ICollection<Genre> Genres { get; set; } = new List<Genre>();
        public List<GameGenre> GameGenres { get; set; }
        public List<Rating> Ratings { get; set; }
    }
}
