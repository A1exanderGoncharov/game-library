using BLL.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class CreateGameViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string HeaderImage { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Rating { get; set; }
        public ICollection<GameGenreDTO> GameGenres { get; set; }
        public string Trailer { get; set; }
        public string Developer { get; set; }
        public CommentDTO Comment { get; set; }
    }
}
