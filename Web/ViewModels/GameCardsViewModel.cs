using BLL.DTO;
using System.Collections.Generic;
using Web.Helpers;

namespace Web.ViewModels
{
    public class GameCardsViewModel
    {
        public IEnumerable<GameDTO> Games = new List<GameDTO>();
        public IEnumerable<GenreDTO> Genres = new List<GenreDTO>();
        public PaginatedList<GameDTO> PaginatedGames { get; set; }
        public IEnumerable<GameDTO> recommendedGamesFirstRange = new List<GameDTO>();
        public IEnumerable<GameDTO> recommendedGamesSecondRange = new List<GameDTO>();
        public IEnumerable<GameDTO> recommendedGamesThirdRange = new List<GameDTO>();
        public string ActionName { get; set; }
        public int? GameGenreId { get; set; }
        public string SearchString { get; set; }
    }
}
