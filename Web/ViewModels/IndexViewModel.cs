using BLL.DTO;
using System.Collections.Generic;
using Web.Helpers;

namespace Web.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<GameDTO> games = new List<GameDTO>();
        public IEnumerable<GenreDTO> genres = new List<GenreDTO>();
        public IEnumerable<GameDTO> recommendedGamesFirstRange = new List<GameDTO>();
        public IEnumerable<GameDTO> recommendedGamesSecondRange = new List<GameDTO>();
        public IEnumerable<GameDTO> recommendedGamesThirdRange = new List<GameDTO>();
        public PaginatedList<GameDTO> PaginatedGames { get; set; }
    }
}
