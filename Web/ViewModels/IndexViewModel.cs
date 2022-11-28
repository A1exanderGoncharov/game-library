using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<GameDTO> games = new List<GameDTO>();
        public IEnumerable<GenreDTO> genres = new List<GenreDTO>();
    }
}
