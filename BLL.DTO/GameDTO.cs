using System;
using System.Collections.Generic;

namespace BLL.DTO
{
    public class GameDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string HeaderImage { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public string Rating { get; set; }
        public string Trailer { get; set; }
        public string Developer { get; set; }

        public List<GameGenreDTO> GameGenres { get; set; }
        public ICollection<CommentDTO> Comments { get; set; }      
        public ApplicationUserDTO ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        public List<RatingDTO> Ratings { get; set; }
    }
}
